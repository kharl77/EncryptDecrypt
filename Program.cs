// Decompiled with JetBrains decompiler
// Type: EncryptDecrypt.Program
// Assembly: EncryptDecrypt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AEC36668-A4C2-41A0-AF71-BB422E4FB67D
// Assembly location: C:\Program Files (x86)\Kharl Lampaoug Corporation\Crypthography\EncryptDecrypt.exe

using System;
using System.Windows.Forms;

namespace EncryptDecrypt
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new frmMain());
    }
  }
}
