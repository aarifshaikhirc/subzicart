using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace subzicart.Models
{
    
    public class commonFunctions
    {
        
        public IEnumerable<tblCategory> GetCategory()
        {
            var context = new subzicartEntities();
            return (from p in context.Set<tblCategory>()
                    //where p.CategoryID == categoryID
                    select new { catId = p.catId, catName = p.catName }).ToList()
                   .Select(x => new tblCategory { catId = x.catId, catName = x.catName });
        }

        public IEnumerable<tblSubCategory> GetSubCategory()
        {
            var context = new subzicartEntities();
            return (from p in context.Set<tblSubCategory>()
                        //where p.CategoryID == categoryID
                    select new { subCatId = p.subCatId, subCatName = p.subCatName }).ToList()
                   .Select(x => new tblSubCategory { subCatId = x.subCatId, subCatName = x.subCatName });
        }

        public IEnumerable<tblBrandPrefix> GetBrand()
        {
            var context = new subzicartEntities();
            return (from p in context.Set<tblBrandPrefix>()
                        //where p.CategoryID == categoryID
                    select new { brand_id = p.brand_id, brand_name = p.brand_name }).ToList()
                   .Select(x => new tblBrandPrefix { brand_id = x.brand_id, brand_name = x.brand_name });
        }
    }
}