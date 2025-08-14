namespace TC.Core.Define.Class
{
    /// <summary>
    /// 状态机基础数据（纯数据，不依赖 Unity）
    /// </summary>
    public class TC_Define_Class_CGameStateData
    {
        public TC_Define_Class_EGameState LastState { get; set; } = TC_Define_Class_EGameState.Start;
        public TC_Define_Class_EGameState CurrentState { get; set; } = TC_Define_Class_EGameState.Start;
    }
}
