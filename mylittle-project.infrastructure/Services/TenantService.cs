using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;

namespace mylittle_project.infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FeatureModuleDto>> GetFeatureTreeAsync(Guid tenantId)
        {
            var modules = await _unitOfWork.FeatureModules.GetAll()
                                           .Include(m => m.Features)
                                           .AsNoTracking()
                                           .ToListAsync();

            var tenantModules = await _unitOfWork.TenantFeatureModules.GetAll()
                                                 .Where(tm => tm.TenantId == tenantId)
                                                 .ToDictionaryAsync(tm => tm.ModuleId);

            var tenantFeatures = await _unitOfWork.TenantFeatures.GetAll()
                                                  .Where(tf => tf.TenantId == tenantId)
                                                  .ToDictionaryAsync(tf => tf.FeatureId);

            return modules.Select(module => new FeatureModuleDto
            {
                ModuleId = module.Id,
                Name = module.Name,
                IsEnabled = tenantModules.TryGetValue(module.Id, out var tModule) && tModule.IsEnabled,
                Features = module.Features.Select(feature => new FeatureToggleDto
                {
                    FeatureId = feature.Id,
                    Name = feature.Name,
                    IsEnabled = tenantFeatures.TryGetValue(feature.Id, out var tFeature) && tFeature.IsEnabled
                }).ToList()
            }).ToList();
        }


        public async Task<bool> UpdateModuleToggleAsync(Guid tenantId, Guid moduleId, bool isEnabled)
        {
            var tModule = await _unitOfWork.TenantFeatureModules.GetAll()
                                           .Include(tm => tm.TenantFeatures)
                                           .FirstOrDefaultAsync(tm =>
                                               tm.TenantId == tenantId && tm.ModuleId == moduleId);

            if (tModule == null)
                return false;

            tModule.IsEnabled = isEnabled;

            foreach (var feature in tModule.TenantFeatures)
            {
                feature.IsEnabled = isEnabled && feature.IsEnabled;
            }

            await _unitOfWork.SaveAsync();
            return true;
        }



        public async Task<bool> UpdateFeatureToggleAsync(Guid tenantId, Guid featureId, bool isEnabled)
        {
            var tFeature = await _unitOfWork.TenantFeatures.GetAll()
                                            .FirstOrDefaultAsync(tf =>
                                                tf.TenantId == tenantId && tf.FeatureId == featureId);

            if (tFeature == null)
                return false;

            var parentModule = await _unitOfWork.TenantFeatureModules.GetAll()
                                                .FirstOrDefaultAsync(tm =>
                                                    tm.TenantId == tenantId && tm.ModuleId == tFeature.ModuleId);

            if (parentModule == null || (!parentModule.IsEnabled && isEnabled))
                return false;

            tFeature.IsEnabled = isEnabled;
            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _unitOfWork.Tenants.GetAll()
                .Include(t => t.AdminUser)
                .Include(t => t.Store)
                .Include(t => t.Branding)
                .Include(t => t.ContentSettings)
                .Include(t => t.DomainSettings)
                .Include(t => t.FeatureModules)
                .Include(t => t.Features)
                .ToListAsync();
        }

        public async Task<Tenant> CreateAsync(TenantDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var tenantId = Guid.NewGuid();

                var tenant = new Tenant
                {
                    Id = tenantId,
                    Name = dto.TenantName,
                    TenantName = dto.TenantName,
                    TenantNickname = dto.TenantNickname,
                    Subdomain = dto.Subdomain,
                    IndustryType = dto.IndustryType,
                    Status = dto.Status,
                    Description = dto.Description,
                    IsActive = dto.IsActive,
                    LastAccessed = DateTime.UtcNow,
                    AdminUser = new AdminUser
                    {
                        FullName = dto.AdminUser.FullName,
                        Email = dto.AdminUser.Email,
                        Password = dto.AdminUser.Password,
                        Role = dto.AdminUser.Role,
                        PhoneNumber = dto.AdminUser.PhoneNumber,
                        CountryCode = dto.AdminUser.CountryCode,
                        DateOfBirth = dto.AdminUser.DateOfBirth,
                        Gender = dto.AdminUser.Gender,
                        StreetAddress = dto.AdminUser.StreetAddress,
                        City = dto.AdminUser.City,
                        StateProvince = dto.AdminUser.StateProvince,
                        ZipPostalCode = dto.AdminUser.ZipPostalCode,
                        Country = dto.AdminUser.Country
                    },
                    Store = new Store
                    {
                        Currency = dto.Store.Currency,
                        Language = dto.Store.Language,
                        Timezone = dto.Store.Timezone,
                        UnitSystem = dto.Store.UnitSystem,
                        TextDirection = dto.Store.TextDirection,
                        NumberFormat = dto.Store.NumberFormat,
                        DateFormat = dto.Store.DateFormat,
                        EnableTaxCalculations = dto.Store.EnableTaxCalculations,
                        EnableShipping = dto.Store.EnableShipping,
                        EnableFilters = dto.Store.EnableFilters,
                        TenantId = tenantId
                    },
                    Branding = new Branding
                    {
                        PrimaryColor = dto.Branding.PrimaryColor,
                        AccentColor = dto.Branding.AccentColor,
                        BackgroundColor = dto.Branding.BackgroundColor,
                        SecondaryColor = dto.Branding.SecondaryColor,
                        TextColor = dto.Branding.TextColor,
                        Text = new BrandingText
                        {
                            FontName = dto.Branding.TextSettings.FontName,
                            FontSize = dto.Branding.TextSettings.FontSize,
                            FontWeight = dto.Branding.TextSettings.FontWeight
                        },
                        Media = new BrandingMedia
                        {
                            LogoUrl = dto.Branding.MediaSettings.LogoUrl,
                            FaviconUrl = dto.Branding.MediaSettings.FaviconUrl,
                            BackgroundImageUrl = dto.Branding.MediaSettings.BackgroundImageUrl
                        },
                        ColorPresets = dto.Branding.ColorPresets?.Select(p => new ColorPreset
                        {
                            Name = p.Name,
                            PrimaryColor = p.PrimaryColor,
                            SecondaryColor = p.SecondaryColor,
                            AccentColor = p.AccentColor
                        }).ToList()
                    },
                    ContentSettings = new ContentSettings
                    {
                        WelcomeMessage = dto.ContentSettings.WelcomeMessage,
                        CallToAction = dto.ContentSettings.CallToAction,
                        HomePageContent = dto.ContentSettings.HomePageContent,
                        AboutUs = dto.ContentSettings.AboutUs,
                        ContactUs = dto.ContentSettings.ContactUs,
                        TermsAndPrivacyPolicy = dto.ContentSettings.TermsAndPrivacyPolicy
                    },
                    DomainSettings = new DomainSettings
                    {
                        Subdomain = dto.DomainSettings.Subdomain,
                        MainDomain = dto.DomainSettings.MainDomain,
                        CustomDomain = dto.DomainSettings.CustomDomain,
                        EnableApiAccess = dto.DomainSettings.EnableApiAccess
                    }
                };

                _unitOfWork.Tenants.Add(tenant);

                // Seed Tenant Features
                await SeedTenantFeaturesAsync(tenantId);

                // Add Tenant Subscriptions
                if (dto.TenantSubscriptions != null && dto.TenantSubscriptions.Any())
                {
                    foreach (var plan in dto.TenantSubscriptions)
                    {
                        var globalPlan = await _unitOfWork.GlobalSubscriptions.GetAll()
                            .FirstOrDefaultAsync(gp => gp.PlanName.ToLower().Trim() == plan.PlanName.ToLower().Trim());

                        if (globalPlan == null)
                            throw new Exception($"Global plan '{plan.PlanName}' not found in GlobalSubscriptions.");

                        await _unitOfWork.TenantSubscriptions.AddAsync(new TenantSubscription
                        {
                            Id = Guid.NewGuid(),
                            TenantId = tenantId,
                            GlobalPlanId = globalPlan.Id,
                            PlanName = plan.PlanName,
                            PlanCost = plan.PlanCost,
                            NumberOfAds = plan.NumberOfAds,
                            MaxEssentialMembers = plan.MaxEssentialMembers,
                            MaxPremiumMembers = plan.MaxPremiumMembers,
                            MaxEliteMembers = plan.MaxEliteMembers,
                            IsTrial = plan.IsTrial,
                            IsActive = true
                        });
                    }
                }
                else
                {
                    await CloneDefaultGlobalPlansToTenantAsync(tenantId);
                }

                // Save all changes in one transaction
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return tenant;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }



        private async Task SeedTenantFeaturesAsync(Guid tenantId)
        {
            var modules = await _unitOfWork.FeatureModules.GetAll()
                                           .Include(m => m.Features)
                                           .ToListAsync();

            foreach (var module in modules)
            {
                await _unitOfWork.TenantFeatureModules.AddAsync(new TenantFeatureModule
                {
                    TenantId = tenantId,
                    ModuleId = module.Id,
                    IsEnabled = false
                });

                foreach (var feature in module.Features)
                {
                    await _unitOfWork.TenantFeatures.AddAsync(new TenantFeature
                    {
                        TenantId = tenantId,
                        FeatureId = feature.Id,
                        ModuleId = module.Id,
                        IsEnabled = false
                    });
                }
            }
            // Do not save here
        }

        private async Task CloneDefaultGlobalPlansToTenantAsync(Guid tenantId)
        {
            var globalPlans = await _unitOfWork.GlobalSubscriptions.GetAll().ToListAsync();

            foreach (var plan in globalPlans)
            {
                await _unitOfWork.TenantSubscriptions.AddAsync(new TenantSubscription
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    GlobalPlanId = plan.Id,
                    PlanName = plan.PlanName,
                    PlanCost = plan.PlanCost,
                    NumberOfAds = plan.NumberOfAds,
                    MaxEssentialMembers = plan.MaxEssentialMembers,
                    MaxPremiumMembers = plan.MaxPremiumMembers,
                    MaxEliteMembers = plan.MaxEliteMembers,
                    IsTrial = plan.IsTrial,
                    IsActive = true
                });
            }
            // Do not save here
        }

        public async Task<bool> UpdateTenantAsync(Guid tenantId, TenantDto dto)
        {
            var tenant = await _unitOfWork.Tenants.GetAll()
                 .Include(t => t.AdminUser)
                 .Include(t => t.Store)
                 .Include(t => t.Branding).ThenInclude(b => b.Text)
                 .Include(t => t.Branding).ThenInclude(b => b.Media)
                 .Include(t => t.Branding).ThenInclude(b => b.ColorPresets)
                 .Include(t => t.ContentSettings)
                 .Include(t => t.DomainSettings)
                 .FirstOrDefaultAsync(t => t.Id == tenantId);

            if (tenant == null)
                return false;

            tenant.TenantName = dto.TenantName;
            tenant.TenantNickname = dto.TenantNickname;
            tenant.Name = dto.TenantName;
            tenant.Subdomain = dto.Subdomain;
            tenant.IndustryType = dto.IndustryType;
            tenant.Status = dto.Status;
            tenant.Description = dto.Description;
            tenant.IsActive = dto.IsActive;
            tenant.LastAccessed = DateTime.UtcNow;

            if (tenant.AdminUser != null)
            {
                tenant.AdminUser.FullName = dto.AdminUser.FullName;
                tenant.AdminUser.Email = dto.AdminUser.Email;
                tenant.AdminUser.Password = dto.AdminUser.Password;
                tenant.AdminUser.Role = dto.AdminUser.Role;
                tenant.AdminUser.PhoneNumber = dto.AdminUser.PhoneNumber;
                tenant.AdminUser.CountryCode = dto.AdminUser.CountryCode;
                tenant.AdminUser.DateOfBirth = dto.AdminUser.DateOfBirth;
                tenant.AdminUser.Gender = dto.AdminUser.Gender;
                tenant.AdminUser.StreetAddress = dto.AdminUser.StreetAddress;
                tenant.AdminUser.City = dto.AdminUser.City;
                tenant.AdminUser.StateProvince = dto.AdminUser.StateProvince;
                tenant.AdminUser.ZipPostalCode = dto.AdminUser.ZipPostalCode;
                tenant.AdminUser.Country = dto.AdminUser.Country;
            }

            if (tenant.Store != null)
            {
                tenant.Store.Country = dto.Store.Country;
                tenant.Store.Currency = dto.Store.Currency;
                tenant.Store.Language = dto.Store.Language;
                tenant.Store.Timezone = dto.Store.Timezone;
                tenant.Store.UnitSystem = dto.Store.UnitSystem;
                tenant.Store.TextDirection = dto.Store.TextDirection;
                tenant.Store.NumberFormat = dto.Store.NumberFormat;
                tenant.Store.DateFormat = dto.Store.DateFormat;
                tenant.Store.EnableTaxCalculations = dto.Store.EnableTaxCalculations;
                tenant.Store.EnableShipping = dto.Store.EnableShipping;
                tenant.Store.EnableFilters = dto.Store.EnableFilters;
            }

            if (tenant.Branding != null)
            {
                tenant.Branding.PrimaryColor = dto.Branding.PrimaryColor;
                tenant.Branding.SecondaryColor = dto.Branding.SecondaryColor;
                tenant.Branding.AccentColor = dto.Branding.AccentColor;
                tenant.Branding.BackgroundColor = dto.Branding.BackgroundColor;
                tenant.Branding.TextColor = dto.Branding.TextColor;

                if (tenant.Branding.Text != null)
                {
                    tenant.Branding.Text.FontName = dto.Branding.TextSettings.FontName;
                    tenant.Branding.Text.FontSize = dto.Branding.TextSettings.FontSize;
                    tenant.Branding.Text.FontWeight = dto.Branding.TextSettings.FontWeight;
                }

                if (tenant.Branding.Media != null)
                {
                    tenant.Branding.Media.LogoUrl = dto.Branding.MediaSettings.LogoUrl;
                    tenant.Branding.Media.FaviconUrl = dto.Branding.MediaSettings.FaviconUrl;
                    tenant.Branding.Media.BackgroundImageUrl = dto.Branding.MediaSettings.BackgroundImageUrl;
                }

                var existingPresets = await _unitOfWork.ColorPresets.GetAll()
                     .Where(cp => cp.BrandingId == tenant.Branding.Id)
                     .ToListAsync();

                _unitOfWork.ColorPresets.RemoveRange(existingPresets);

                tenant.Branding.ColorPresets = dto.Branding.ColorPresets?.Select(p => new ColorPreset
                {
                    Name = p.Name,
                    PrimaryColor = p.PrimaryColor,
                    SecondaryColor = p.SecondaryColor,
                    AccentColor = p.AccentColor
                }).ToList() ?? new List<ColorPreset>();
            }

            // ───── Content Settings ─────
            if (tenant.ContentSettings != null)
            {
                tenant.ContentSettings.WelcomeMessage = dto.ContentSettings.WelcomeMessage;
                tenant.ContentSettings.CallToAction = dto.ContentSettings.CallToAction;
                tenant.ContentSettings.HomePageContent = dto.ContentSettings.HomePageContent;
                tenant.ContentSettings.AboutUs = dto.ContentSettings.AboutUs;
                tenant.ContentSettings.ContactUs = dto.ContentSettings.ContactUs;
                tenant.ContentSettings.TermsAndPrivacyPolicy = dto.ContentSettings.TermsAndPrivacyPolicy;
            }

            if (tenant.DomainSettings != null)
            {
                tenant.DomainSettings.Subdomain = dto.DomainSettings.Subdomain;
                tenant.DomainSettings.MainDomain = dto.DomainSettings.MainDomain;
                tenant.DomainSettings.CustomDomain = dto.DomainSettings.CustomDomain;
                tenant.DomainSettings.EnableApiAccess = dto.DomainSettings.EnableApiAccess;
            }

            if (dto.TenantSubscriptions != null && dto.TenantSubscriptions.Any())
            {
                var existingSubs = await _unitOfWork.TenantSubscriptions.GetAll()
                    .Where(ts => ts.TenantId == tenantId)
                    .ToListAsync();


                _unitOfWork.TenantSubscriptions.RemoveRange(existingSubs);

                foreach (var sub in dto.TenantSubscriptions)
                {
                    var globalPlan = await _unitOfWork.GlobalSubscriptions.GetAll()
                    .FirstOrDefaultAsync(gp => gp.PlanName.ToLower().Trim() == sub.PlanName.ToLower().Trim());


                    if (globalPlan == null)
                        throw new Exception($"Global plan '{sub.PlanName}' not found.");

                    var newSub = new TenantSubscription
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId,
                        GlobalPlanId = globalPlan.Id,
                        PlanName = sub.PlanName,
                        PlanCost = sub.PlanCost,
                        NumberOfAds = sub.NumberOfAds,
                        MaxEssentialMembers = sub.MaxEssentialMembers,
                        MaxPremiumMembers = sub.MaxPremiumMembers,
                        MaxEliteMembers = sub.MaxEliteMembers,
                        IsTrial = sub.IsTrial,
                        IsActive = sub.IsActive
                    };

                    await _unitOfWork.TenantSubscriptions.AddAsync(newSub);
                }
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Tenant?> GetTenantWithFeaturesAsync(Guid tenantId)
        {
            return await _unitOfWork.Tenants.GetAll()
                                     .Include(t => t.FeatureModules)
                                     .ThenInclude(tm => tm.Module)
                                     .Include(t => t.Features)
                                     .ThenInclude(tf => tf.Feature)
                                     .FirstOrDefaultAsync(t => t.Id == tenantId);
        }


        public async Task<bool> UpdateStoreAsync(Guid tenantId, StoreDto dto)
        {
            var tenant = await _unitOfWork.Tenants.GetAll()
                                       .Include(t => t.Store)
                                       .FirstOrDefaultAsync(t => t.Id == tenantId);

            if (tenant == null)
                return false;

            if (tenant.Store == null)
            {
                tenant.Store = new Store { TenantId = tenantId };
                await _unitOfWork.Stores.AddAsync(tenant.Store);
            }

            tenant.Store.Country = dto.Country;
            tenant.Store.Currency = dto.Currency;
            tenant.Store.Language = dto.Language;
            tenant.Store.Timezone = dto.Timezone;
            tenant.Store.UnitSystem = dto.UnitSystem;
            tenant.Store.TextDirection = dto.TextDirection;
            tenant.Store.NumberFormat = dto.NumberFormat;
            tenant.Store.DateFormat = dto.DateFormat;
            tenant.Store.EnableTaxCalculations = dto.EnableTaxCalculations;
            tenant.Store.EnableShipping = dto.EnableShipping;
            tenant.Store.EnableFilters = dto.EnableFilters;

            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<List<PortalSummaryDto>> GetPortalSummariesAsync()
        {
            return await _unitOfWork.Tenants.GetAll()
                .Select(t => new PortalSummaryDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsActive = t.IsActive,
                    LastAccessed = t.LastAccessed
                })
                .ToListAsync();
        }



        public async Task<PaginatedResult<TenantDto>> GetPaginatedAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Tenants.GetAll().Select(t => new TenantDto
            {
                Id = t.Id,
                TenantName = t.TenantName,
            });

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<TenantDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }


    }
}
