//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GymManagement
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class PrzypisaneDiety
    {
        public int idPD { get; set; }
        public int idDieta { get; set; }
        public int idKlient { get; set; }
    
        public virtual Diety Diety { get; set; }
        public virtual Klienci Klienci { get; set; }
    }
}
