
public interface IState<T> 
{
    void OnEnter(T character);
    void OnExecute(T character);
    void OnExit(T character);
}
