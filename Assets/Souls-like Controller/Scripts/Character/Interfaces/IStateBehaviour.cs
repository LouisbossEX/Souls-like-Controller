public interface IStateBehaviour
{
    void OnEnter();
    void Update(float deltaTime);
    void OnExit();
    StateData GetStateData();
}