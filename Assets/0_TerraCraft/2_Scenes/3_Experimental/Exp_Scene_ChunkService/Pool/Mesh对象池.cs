using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public static class Mesh对象池
    {
        private static Stack<Mesh> pool = new Stack<Mesh>();

        /// <summary>
        /// 获取一个可复用的 Mesh，如果没有则创建一个新的
        /// </summary>
        public static Mesh Get()
        {
            return pool.Count > 0 ? pool.Pop() : new Mesh();
        }

        /// <summary>
        /// 回收 Mesh，必须先 Clear 后放回池
        /// </summary>
        public static void Recycle(Mesh mesh)
        {
            if (mesh == null)
                return;

            mesh.Clear();
            pool.Push(mesh);
        }

        /// <summary>
        /// 销毁池中所有未使用的 Mesh
        /// </summary>
        public static void ClearAll()
        {
            foreach (var mesh in pool)
            {
                Object.Destroy(mesh); 
            }
            pool.Clear();
        }

        /// <summary>
        /// 当前池中缓存的数量（可选）
        /// </summary>
        public static int Count => pool.Count;
    }

}
