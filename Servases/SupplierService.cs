using AutoMapper;
using InventoryManagementWithExpirationDatesSystem.Database;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Interfacese;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementWithExpirationDatesSystem.Servases
{
    public class SupplierService : ISupplierService
    {
        private readonly WarehouseManagementSystemContext _context;
        private readonly IMapper _mapper;    

        public SupplierService(WarehouseManagementSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SupplierDTO> AddSuppliersAsync(SupplierDTO supplierDTO)
        {
            var supplier = _mapper.Map<Supplier>(supplierDTO);
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return _mapper.Map<SupplierDTO>(supplier) ;
        }

        public async Task<bool> DeleteSuppliersAsync(int supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);  
            if (supplier == null )  return false; 

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.Include(s => s.Stocks).ToListAsync();
            return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }

        public async Task<SupplierDTO> GetsupplierByIdAsync(int supplierId)
        { 
            var supplier = await _context.Suppliers.Include(s => s.Stocks)
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId);
            return supplier == null ? null : _mapper.Map<SupplierDTO>(supplier);

        }

        public async Task<SupplierDTO> UpdateSuppliersAsync(int supplierId, SupplierDTO supplierDTO)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);

            if (supplier == null) return null;

            _mapper.Map(supplierDTO, supplier);
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return _mapper.Map<SupplierDTO>(supplier);  

        }
    }
}
