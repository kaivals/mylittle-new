using Microsoft.EntityFrameworkCore.Storage;
using mylittle_project.Application.Interfaces;
using mylittle_project.Application.Interfaces.Repositories;
using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;

namespace mylittle_project.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Tenants = new GenericRepository<Tenant>(_context);
            AdminUsers = new GenericRepository<AdminUser>(_context);
            Stores = new GenericRepository<Store>(_context);
            Brandings = new GenericRepository<Branding>(_context);
            BrandingTexts = new GenericRepository<BrandingText>(_context);
            BrandingMedia = new GenericRepository<BrandingMedia>(_context);
            ContentSettings = new GenericRepository<ContentSettings>(_context);
            DomainSettings = new GenericRepository<DomainSettings>(_context);
            ActivityLogs = new GenericRepository<ActivityLogBuyer>(_context);
            ColorPresets = new GenericRepository<ColorPreset>(_context);
            Products = new GenericRepository<Product>(_context);
            Orders = new GenericRepository<Order>(_context);
            OrderItems = new GenericRepository<OrderItem>(_context);
            Buyers = new GenericRepository<Buyer>(_context);
            Dealers = new GenericRepository<Dealer>(_context);
            DealerSubscriptions = new GenericRepository<DealerSubscription>(_context);
            UserDealers = new GenericRepository<UserDealer>(_context);
            PortalAssignments = new GenericRepository<PortalAssignment>(_context);
            VirtualNumberAssignments = new GenericRepository<VirtualNumberAssignment>(_context);
            KycDocumentRequests = new GenericRepository<KycDocumentRequest>(_context);
            KycDocumentUploads = new GenericRepository<KycDocumentUpload>(_context);
            TenentPortalLinks = new GenericRepository<TenentPortalLink>(_context);
            FeatureModules = new GenericRepository<FeatureModule>(_context);
            Features = new GenericRepository<Feature>(_context);
            TenantFeatureModules = new GenericRepository<TenantFeatureModule>(_context);
            TenantFeatures = new GenericRepository<TenantFeature>(_context);
            Categories = new GenericRepository<Category>(_context);
            GlobalSubscriptions = new GenericRepository<GlobalSubscription>(_context);
            TenantSubscriptions = new GenericRepository<TenantSubscription>(_context);
            TenantPlanAssignments = new GenericRepository<TenantPlanAssignment>(_context);
            Filters = new GenericRepository<Filter>(_context);

            // ✅ Added Dynamic Product Repositories
            ProductFields = new GenericRepository<ProductField>(_context);
            ProductSections = new GenericRepository<ProductSection>(_context);


            Brands = new GenericRepository<Brand>(_context); // ✅ Add this


            ProductReviews = new GenericRepository<ProductReview>(_context);

            ProductTags = new GenericRepository<ProductTag>(_context);

            ProductAttributes = new GenericRepository<ProductAttribute>(_context); // ✅ Correct

        }

        public IGenericRepository<Tenant> Tenants { get; }
        public IGenericRepository<AdminUser> AdminUsers { get; }
        public IGenericRepository<Store> Stores { get; }
        public IGenericRepository<Branding> Brandings { get; }
        public IGenericRepository<BrandingText> BrandingTexts { get; }
        public IGenericRepository<BrandingMedia> BrandingMedia { get; }
        public IGenericRepository<ContentSettings> ContentSettings { get; }
        public IGenericRepository<DomainSettings> DomainSettings { get; }
        public IGenericRepository<ActivityLogBuyer> ActivityLogs { get; }
        public IGenericRepository<ColorPreset> ColorPresets { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Order> Orders { get; }
        public IGenericRepository<OrderItem> OrderItems { get; }
        public IGenericRepository<Buyer> Buyers { get; }
        public IGenericRepository<Dealer> Dealers { get; }
        public IGenericRepository<DealerSubscription> DealerSubscriptions { get; }
        public IGenericRepository<UserDealer> UserDealers { get; }
        public IGenericRepository<PortalAssignment> PortalAssignments { get; }
        public IGenericRepository<VirtualNumberAssignment> VirtualNumberAssignments { get; }
        public IGenericRepository<KycDocumentRequest> KycDocumentRequests { get; }
        public IGenericRepository<KycDocumentUpload> KycDocumentUploads { get; }
        public IGenericRepository<TenentPortalLink> TenentPortalLinks { get; }
        public IGenericRepository<FeatureModule> FeatureModules { get; }
        public IGenericRepository<Feature> Features { get; }
        public IGenericRepository<TenantFeatureModule> TenantFeatureModules { get; }
        public IGenericRepository<TenantFeature> TenantFeatures { get; }
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<GlobalSubscription> GlobalSubscriptions { get; }
        public IGenericRepository<TenantSubscription> TenantSubscriptions { get; }
        public IGenericRepository<TenantPlanAssignment> TenantPlanAssignments { get; }
        public IGenericRepository<Filter> Filters { get; }

        // ✅ Added
        public IGenericRepository<ProductField> ProductFields { get; }
        public IGenericRepository<ProductSection> ProductSections { get; }

        public IGenericRepository<Brand> Brands { get; } // ✅ Add this

        public IGenericRepository<ProductReview> ProductReviews { get; }

        public IGenericRepository<ProductTag> ProductTags { get; }

        public IGenericRepository<ProductAttribute> ProductAttributes { get; }



        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
