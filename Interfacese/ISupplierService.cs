using InventoryManagementWithExpirationDatesSystem.DTOs;

namespace InventoryManagementWithExpirationDatesSystem.Interfacese
{
    public interface ISupplierService
    {
        Task<SupplierDTO> GetsupplierByIdAsync(int supplierId);
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task<SupplierDTO> AddSuppliersAsync(SupplierDTO supplierDTO);
        Task<SupplierDTO> UpdateSuppliersAsync(int supplierId, SupplierDTO supplierDTO);
        Task<bool> DeleteSuppliersAsync(int supplierId);


    }
}
