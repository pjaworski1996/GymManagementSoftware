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
    
    public partial class Produkty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produkty()
        {
            this.Transakcje = new ObservableCollection<Transakcje>();
        }
    
        public int idProdukt { get; set; }
        public string nazwa { get; set; }
        public Nullable<int> cena { get; set; }
        public int ilosc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Transakcje> Transakcje { get; set; }
    }
}