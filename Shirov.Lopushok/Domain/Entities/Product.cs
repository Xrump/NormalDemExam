using Microsoft.EntityFrameworkCore;
using Shirov.Lopushok.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shirov.Lopushok.Domain.Entities
{
    public partial class Product
    {
        public Product()
        {
            ProductCostHistories = new HashSet<ProductCostHistory>();
            ProductMaterials = new HashSet<ProductMaterial>();
            ProductSales = new HashSet<ProductSale>();
        }
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? ProductTypeId { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? ProductionPersonCount { get; set; }
        public int? ProductionWorkshopNumber { get; set; }
        public decimal MinCostForAgent { get; set; }
        public string Materials
        {
            get
            {
                List<Material> materials = new List<Material>();
                List<ProductMaterial> productMaterials = new ApplicationDbContext().ProductMaterials
                    .Include(pm => pm.Material)
                    .Where(pm => pm.ProductId == this.Id)
                    .ToList();

                foreach (ProductMaterial productMaterial in productMaterials)
                    materials.Add(productMaterial.Material);

                int materialsInRowCount = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append((materials.Count > 0) ? "Материалы: " : "Нет материалов");

                for (int i = 0; i < materials.Count; i++)
                {
                    if (materialsInRowCount % 3 == 0 && materialsInRowCount != 0)
                    {
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append("\r\n");
                    }
                    sb.Append($"{materials[i].Title}, ");
                    materialsInRowCount++;
                }
                if (materials.Count > 0)
                    sb.Remove(sb.Length - 2, 2);

                return sb.ToString();
            }
        }
        public decimal Cost
        {
            get
            {
                    if (Materials == "Нет материалов")
                        return MinCostForAgent;

                    List<Material> materials = new List<Material>();
                    List<ProductMaterial> productMaterials = new ApplicationDbContext().ProductMaterials
                        .Include(pm => pm.Material)
                        .Where(pm => pm.ProductId == this.Id)
                        .ToList();

                    foreach (ProductMaterial productMaterial in productMaterials)
                        materials.Add(productMaterial.Material);

                    decimal cost = 0;

                    for (int i = 0; i < materials.Count; i++)
                    {
                        cost += materials[i].Cost * Convert.ToDecimal(productMaterials[i].Count);
                    }

                    return cost;

            }
        }
        public string NormalImage
        {get => (Image == null) ? $"..\\Resources\\picture.png" : $"..\\Resources{Image}";}

        public virtual ProductType? ProductType { get; set; }
        public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}
