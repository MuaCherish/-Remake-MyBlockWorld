using UnityEngine;
using System.Collections.Generic;

namespace Tc.Exp.ChunkService
{
    /// <summary>
    /// UI管理器，用于集中控制所有UI面板的加载、打开、关闭与管理
    /// 支持 ScriptableObject 配置表，不同场景可以指定不同配置
    /// </summary>
    public class UI管理器 : MonoBehaviour
    {
        //当前场景的 UI 预制体配置表
        public UI配置表 当前UI配置表;

        //已经实例化的 UI 面板对象池
        private Dictionary<string, GameObject> 已打开的面板 = new Dictionary<string, GameObject>();

        //UI父节点
        public Transform UIPanelParent;

        /// <summary>
        /// 打开一个 UI 面板（通过名称）
        /// 如果该面板已打开，则不重复打开
        /// </summary>
        /// <param name="panelName">面板在配置表中的名称</param>
        public void OpenPanel(string panelName)
        {
            // [注释]：
            // 1. 如果已在字典中存在，则直接返回
            // 2. 否则从配置表中查找预制体路径
            // 3. 通过 Resources.Load 或 Addressables 加载预制体
            // 4. 实例化预制体并加入到父节点下
            // 5. 加入到已打开面板字典中
        }

        /// <summary>
        /// 关闭一个 UI 面板（通过名称）
        /// 如果该面板未打开，则不做任何操作
        /// </summary>
        /// <param name="panelName">面板在配置表中的名称</param>
        public void ClosePanel(string panelName)
        {
            // [注释]：
            // 1. 判断字典中是否存在该面板
            // 2. 如果存在，则销毁GameObject（或隐藏它）
            // 3. 从字典中移除该面板
        }

        /// <summary>
        /// 判断指定面板是否处于已打开状态
        /// </summary>
        public bool IsPanelOpen(string panelName)
        {
            // [注释]：
            // 直接查询字典是否包含该key
            return false;
        }

        /// <summary>
        /// 关闭所有当前打开的面板（可用于切场景或重置UI）
        /// </summary>
        public void CloseAllPanels()
        {
            // [注释]：
            // 遍历字典中所有面板并依次销毁或隐藏
            // 清空字典
        }

        /// <summary>
        /// 根据配置表动态加载并缓存某个面板（异步或预加载用）
        /// </summary>
        public void PreloadPanel(string panelName)
        {
            // [注释]：
            // 用于在切场景前或游戏过程中提前加载某些常用面板
        }
    }
}
