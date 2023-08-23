using System.IO;
using System.Security.Cryptography;

namespace ET
{
	public static class MD5Helper
	{
		/// <summary>
		/// 将文件路径MD5加密后转小写16进制
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string FileMD5(string filePath)
		{
			byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
	            MD5 md5 = MD5.Create();
				retVal = md5.ComputeHash(file);
			}
			return retVal.ToHex("x2");
		}
	}
}
