//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stanochki.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Requests
    {
        public int id_req { get; set; }
        public int id_price { get; set; }
        public int client { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual Entry Entry { get; set; }
        public virtual Entry Entry1 { get; set; }
        public virtual Prices Prices { get; set; }
    }
}
