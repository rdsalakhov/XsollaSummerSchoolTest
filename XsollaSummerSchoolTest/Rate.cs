//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XsollaSummerSchoolTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rate
    {
        public int Id { get; set; }
        public string SessionString { get; set; }
        public short Mark { get; set; }
    
        public virtual NewsItem NewsItem { get; set; }
    }
}
