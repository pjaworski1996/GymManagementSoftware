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
    
    public partial class Treningi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Treningi()
        {
            this.ZarezerwowaneTreningi = new ObservableCollection<ZarezerwowaneTreningi>();
        }
    
        public int idTrening { get; set; }
        public string nazwa { get; set; }
        public System.DateTime dataTreningu { get; set; }
        public int idTrener { get; set; }
        public int idSilownia { get; set; }
    
        public virtual Silownie Silownie { get; set; }
        public virtual Trenerzy Trenerzy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<ZarezerwowaneTreningi> ZarezerwowaneTreningi { get; set; }
    }
}
