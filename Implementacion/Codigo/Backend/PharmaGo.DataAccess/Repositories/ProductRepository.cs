using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.DataAccess.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        private readonly PharmacyGoDbContext _context;

        public ProductRepository(PharmacyGoDbContext context) : base(context)
        {
            this._context = context;
        }

        public override Product GetOneByExpression(Expression<Func<Product, bool>> expression)
        {
            return this._context.Set<Product>().Include("Pharmacy").FirstOrDefault(expression);
        }

        public override void InsertOne(Product product)
        {
            this._context.Entry(product).State = EntityState.Added;
            this._context.Set<Product>().Add(product);
        }

        public override IEnumerable<Product> GetAllByExpression(Expression<Func<Product, bool>> expression)
        {
            return this._context.Set<Product>().Include(x => x.Pharmacy).Where(expression);
        }
    }
}
