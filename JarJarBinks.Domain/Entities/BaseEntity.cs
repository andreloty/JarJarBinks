using System;
using System.Collections.Generic;
using System.Text;

namespace JarJarBinks.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; protected set; }
        public bool IsActive { get; protected set; }
    }
}
