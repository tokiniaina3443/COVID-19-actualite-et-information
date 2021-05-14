using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Coronavirus_Information_v3.Models
{
    public class Base64
    {
        string nom;
        string data;

        public Base64()
        {
        }

        public Base64(string nom, string data)
        {
            this.nom = nom;
            this.data = data;
        }

        public Illustration ToIllustration()
        {
            string base64 = this.data;
            byte[] bytes = Convert.FromBase64String(base64);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                image.Save("wwwroot/images/data/" + this.nom + ".jpg", ImageFormat.Jpeg);
            }
            Illustration illustration = new Illustration(this.nom, "/images/data/" + this.nom + ".jpg");
            illustration.Id = illustration.Save();
            return illustration;
        }
    }
}
