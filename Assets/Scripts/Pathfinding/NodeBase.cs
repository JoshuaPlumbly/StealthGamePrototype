public abstract class NodeBase<T>
{
    public T[] Neighbors { get; set; }

    public T Connection { get; set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;

    public void SetConnection(T node) => Connection = node;
    public void SetG(int g) => G = g;
    public void SetH(int h) => H = h;

    public abstract int GetDistanceTo(T node);
}