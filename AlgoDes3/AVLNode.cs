namespace AlgoDes3
{
    public class AVLNode
    {
        public int Key { get; set; }
        public string Data { get; set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public AVLNode Parent { get; set; }

        public AVLNode(int key, string data, AVLNode parent)
        {
            Key = key;
            Data = data;
            Parent = parent;
        }
    }
}