using System;

namespace Senoc.Model.Primitives
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}