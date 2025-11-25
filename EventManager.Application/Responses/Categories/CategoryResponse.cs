using EventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Responses.Categories;

public class CategoryResponse
{
    public int CategoryId { get; set; }
    public string Name { get; set; }

    public CategoryResponse()
    {
        
    }

    public CategoryResponse(Category category)
    {
        CategoryId = category.Id;
        Name = category.Name;
    }
}
