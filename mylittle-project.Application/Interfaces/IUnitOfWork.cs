using mylittle_project.Application.Interfaces.Repositories;
using mylittle_project.Domain.Entities;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Tenant> Tenants { get; }
        IGenericRepository<AdminUser> AdminUsers { get; }
        IGenericRepository<Store> Stores { get; }
        IGenericRepository<Branding> Brandings { get; }
        IGenericRepository<BrandingText> BrandingTexts { get; }
        IGenericRepository<BrandingMedia> BrandingMedia { get; }
        IGenericRepository<ContentSettings> ContentSettings { get; }
        IGenericRepository<DomainSettings> DomainSettings { get; }
        IGenericRepository<ActivityLogBuyer> ActivityLogs { get; }
        IGenericRepository<ColorPreset> ColorPresets { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Buyer> Buyers { get; }
        IGenericRepository<Dealer> Dealers { get; }
        IGenericRepository<DealerSubscription> DealerSubscriptions { get; }
        IGenericRepository<UserDealer> UserDealers { get; }
        IGenericRepository<PortalAssignment> PortalAssignments { get; }
        IGenericRepository<VirtualNumberAssignment> VirtualNumberAssignments { get; }
        IGenericRepository<KycDocumentRequest> KycDocumentRequests { get; }
        IGenericRepository<KycDocumentUpload> KycDocumentUploads { get; }
        IGenericRepository<TenentPortalLink> TenentPortalLinks { get; }
        IGenericRepository<FeatureModule> FeatureModules { get; }
        IGenericRepository<Feature> Features { get; }
        IGenericRepository<TenantFeatureModule> TenantFeatureModules { get; }
        IGenericRepository<TenantFeature> TenantFeatures { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<GlobalSubscription> GlobalSubscriptions { get; }
        IGenericRepository<TenantSubscription> TenantSubscriptions { get; }
        IGenericRepository<TenantPlanAssignment> TenantPlanAssignments { get; }
        IGenericRepository<Filter> Filters { get; }

        // 👇 Added for dynamic product fields
        IGenericRepository<ProductField> ProductFields { get; }
        IGenericRepository<ProductSection> ProductSections { get; }


        IGenericRepository<Brand> Brands { get; }

        IGenericRepository<ProductReview> ProductReviews { get; }

        IGenericRepository<ProductTag> ProductTags { get; }

        IGenericRepository<ProductAttribute> ProductAttributes { get; }

        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}