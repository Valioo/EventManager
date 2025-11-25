using EventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Responses.Tags;

public class TagResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public TagResponse()
    {
        
    }

    public TagResponse(Tag tag)
    {
        Id = tag.Id;
        Name = tag.Name;
    }
}
