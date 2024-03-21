Code Description
This C# code is designed to read customer information from an XML file, encrypt the credit card numbers using AES encryption, and hash the passwords using SHA-256. The original XML file is named customer.xml, and the protected version is saved as customer_protected.xml.

The code utilizes the following cryptographic functionalities:

AES Encryption: The Encrypt method takes a plaintext string and encrypts it using AES encryption with a 128-bit key and an initialization vector (IV). The key and IV are hardcoded in the code.
SHA-256 Hashing: The HashPassword method takes a password string, generates a random salt, and then hashes the salted password using the SHA-256 hashing algorithm. The hashed password is returned as a Base64-encoded string.

Usage

Place the customer.xml file in the same directory as the executable or specify the correct path in the code.
Run the program.
The protected XML file customer_protected.xml will be generated in the same directory.
