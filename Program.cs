using System.Security.Cryptography;
using System.Text;
using System.Xml;

class Program
{
    static void Main(string[] args)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("customer.xml");

        XmlNodeList customerNodes = xmlDoc.SelectNodes("/customers/customer");
        foreach (XmlNode customerNode in customerNodes)
        {
            string creditCard = customerNode.SelectSingleNode("creditcard").InnerText;
            string password = customerNode.SelectSingleNode("password").InnerText;


            string encryptedCreditCard = Encrypt(creditCard);


            string hashedPassword = HashPassword(password);


            XmlNode creditCardNode = customerNode.SelectSingleNode("creditcard");
            XmlNode passwordNode = customerNode.SelectSingleNode("password");

            creditCardNode.InnerText = encryptedCreditCard;
            passwordNode.InnerText = hashedPassword;
        }

        xmlDoc.Save("customer_protected.xml");

        Console.WriteLine("XML file has been protected successfully.");
    }

    static string Encrypt(string plainText)
    {
        string iv = "ThisIsAnIV123456";

        byte[] keyBytes = new byte[] {
            0x3F, 0x7A, 0x7D, 0x94, 0x36, 0x0D, 0x4C, 0x2C,
            0x69, 0x18, 0x77, 0x2F, 0x40, 0xF6, 0xA6, 0xEB
        };
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    static string HashPassword(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(saltedPassword);
            return Convert.ToBase64String(hash);
        }
    }

    static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}
