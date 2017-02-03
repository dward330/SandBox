using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SandBox.XMLDemo
{
    /*
     * To Generate the XSD for this POCO:
     *  -Open CMD as Admin
     *  -CD "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\"
     *  -xsd.exe <Project.(dll or exe)> /type:YourNamespace.Type /outputdir:"AbsolutePathtoFolderToSaveFiles"
     */


    [Serializable, XmlRoot("BookDetails")]
    public class BookDetails
    {
        [XmlElement("Title")]
        public String Title { get; set; }

        [XmlElement("Author")]
        public String Author { get; set; }

        [XmlElement("Publisher")]
        public String Publisher { get; set; }

        [XmlElement("NumOfPages")]
        public long NumOfPages { get; set; }

        [XmlElement("Character")]
        public List<String> Characters { get; set; }
    }
}
