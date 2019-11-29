using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Infra.Extensions
{
    public class PrimaryKeyGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;
        public override string Next(EntityEntry entry) => Guid.NewGuid().ToString("N").ToUpper();
    }
}