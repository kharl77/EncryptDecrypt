// Decompiled with JetBrains decompiler
// Type: EncryptDecrypt.Crypto
// Assembly: EncryptDecrypt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AEC36668-A4C2-41A0-AF71-BB422E4FB67D
// Assembly location: C:\Program Files (x86)\Kharl Lampaoug Corporation\Crypthography\EncryptDecrypt.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecrypt
{
  public class Crypto
  {
    private byte[] Key = new byte[32]
    {
      (byte) 123,
      (byte) 217,
      (byte) 19,
      (byte) 11,
      (byte) 24,
      (byte) 26,
      (byte) 85,
      (byte) 45,
      (byte) 114,
      (byte) 184,
      (byte) 27,
      (byte) 162,
      (byte) 37,
      (byte) 112,
      (byte) 222,
      (byte) 209,
      (byte) 241,
      (byte) 24,
      (byte) 175,
      (byte) 144,
      (byte) 173,
      (byte) 53,
      (byte) 196,
      (byte) 29,
      (byte) 24,
      (byte) 26,
      (byte) 17,
      (byte) 218,
      (byte) 131,
      (byte) 236,
      (byte) 53,
      (byte) 209
    };
    private byte[] Vector = new byte[16]
    {
      (byte) 146,
      (byte) 64,
      (byte) 191,
      (byte) 111,
      (byte) 23,
      (byte) 3,
      (byte) 113,
      (byte) 119,
      (byte) 231,
      (byte) 121,
      (byte) 252,
      (byte) 112,
      (byte) 79,
      (byte) 32,
      (byte) 114,
      (byte) 156
    };
    private ICryptoTransform EncryptorTransform;
    private ICryptoTransform DecryptorTransform;
    private UTF8Encoding UTFEncoder;

    public Crypto()
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      this.EncryptorTransform = rijndaelManaged.CreateEncryptor(this.Key, this.Vector);
      this.DecryptorTransform = rijndaelManaged.CreateDecryptor(this.Key, this.Vector);
      this.UTFEncoder = new UTF8Encoding();
    }

    public static byte[] GenerateEncryptionKey()
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.GenerateKey();
      return rijndaelManaged.Key;
    }

    public static byte[] GenerateEncryptionVector()
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.GenerateIV();
      return rijndaelManaged.IV;
    }

    public string EncryptToString(string TextValue)
    {
      return this.ByteArrToString(this.Encrypt(TextValue));
    }

    public byte[] Encrypt(string TextValue)
    {
      byte[] bytes = this.UTFEncoder.GetBytes(TextValue);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, this.EncryptorTransform, CryptoStreamMode.Write);
      cryptoStream.Write(bytes, 0, bytes.Length);
      cryptoStream.FlushFinalBlock();
      memoryStream.Position = 0L;
      byte[] buffer = new byte[memoryStream.Length];
      memoryStream.Read(buffer, 0, buffer.Length);
      cryptoStream.Close();
      memoryStream.Close();
      return buffer;
    }

    public string DecryptString(string EncryptedString)
    {
      return this.Decrypt(this.StrToByteArray(EncryptedString));
    }

    public string Decrypt(byte[] EncryptedValue)
    {
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, this.DecryptorTransform, CryptoStreamMode.Write);
      cryptoStream.Write(EncryptedValue, 0, EncryptedValue.Length);
      cryptoStream.FlushFinalBlock();
      memoryStream.Position = 0L;
      byte[] numArray = new byte[memoryStream.Length];
      memoryStream.Read(numArray, 0, numArray.Length);
      memoryStream.Close();
      return this.UTFEncoder.GetString(numArray);
    }

    public byte[] StrToByteArray(string str)
    {
      if (str.Length == 0)
        throw new Exception("Invalid string value in StrToByteArray");
      byte[] numArray = new byte[str.Length / 3];
      int startIndex = 0;
      int num1 = 0;
      do
      {
        byte num2 = byte.Parse(str.Substring(startIndex, 3));
        numArray[num1++] = num2;
        startIndex += 3;
      }
      while (startIndex < str.Length);
      return numArray;
    }

    public string ByteArrToString(byte[] byteArr)
    {
      string str = "";
      for (int index = 0; index <= byteArr.GetUpperBound(0); ++index)
      {
        byte num = byteArr[index];
        str = num >= (byte) 10 ? (num >= (byte) 100 ? str + num.ToString() : str + "0" + num.ToString()) : str + "00" + num.ToString();
      }
      return str;
    }

    public string returnMD5Hash(string Value)
    {
      byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Value));
      string empty = string.Empty;
      for (int index = 0; index < hash.Length; ++index)
        empty += hash[index].ToString("x2").ToUpper();
      return empty;
    }
  }
}
