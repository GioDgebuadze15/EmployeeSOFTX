using System.Security.Cryptography;

// This class is used to create RSA Keys for Jwt token authentication
// Create key file and move it into PersonTbc.Api

var rsaKey = RSA.Create();
var privateKey = rsaKey.ExportRSAPrivateKey();
File.WriteAllBytes("key", privateKey);