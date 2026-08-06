using System.Text;
using UnityEngine;

namespace Imaginary.Core.Data
{
    public class GameData<T> where T : struct
    {
        private string filepath;
        public string FileName { set => filepath = $"{Application.persistentDataPath}/{value}"; }

        public GameData()
        {
            FileName = "default";
        }

        public GameData(string filename)
        {
            FileName = filename;
        }

        public T Read()
        {
            if (filepath == null) return default;
            if (!System.IO.File.Exists(filepath)) return default;
            byte[] load = System.IO.File.ReadAllBytes(filepath);
            T data = JsonUtility.FromJson<T>(Encoding.UTF8.GetString(load));
            return data;
        }

        public void Write(T data)
        {
            if (filepath == null) throw new System.NullReferenceException("File name is null.");
            string save = JsonUtility.ToJson(data);
            byte[] bytes = Encoding.UTF8.GetBytes(save);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

        public void Remove()
        {
            if (filepath == null) throw new System.NullReferenceException("File name is null.");
            System.IO.File.Delete(filepath);
        }

        public bool Exists()
        {
            return System.IO.File.Exists(filepath);
        }
    }
}
