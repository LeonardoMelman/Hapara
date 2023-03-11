using System.Xml.Linq;

public class Node{
    public int key, value;
    public Node prev, next;
    public Node() { }
    public Node(int k, int v){
        key = k;
        value = v;
    }
}

public class LRUCache{
    int capacity;
    Dictionary<int, Node> cache = new Dictionary<int, Node>();
    Node left = new Node(0, 0);
    Node right = new Node(0, 0);
    public LRUCache(int cap) //Connecting the nodes (Double linked list)
    {
        int maxCapacity = 1000;
        int minCapacity = 1;
        if (cap >= minCapacity && cap <= maxCapacity) //Checking capacity
        {
            capacity = cap;
            left.next = right;
            right.prev = left;
        }
        else throw new InvalidOperationException("Cannot create cache. Capacity must be between " + minCapacity + " and " + maxCapacity);
    }

    public void insert(Node node){
        double maxKey = Math.Pow(10, 3);
        double minKey = 0;
        double maxValue = Math.Pow(10, 5);
        double minValue = 0;

        if (node.key >= 0 && node.key <= maxKey)
        {
            if (node.value >= 0 && node.key <= maxValue)
            {
                Node prev = right.prev;
                Node next = right;
                prev.next = next.prev = node;
                node.next = next;
                node.prev = prev;
            }
            else {
                throw new InvalidOperationException("Cannot insert value. It must be between " + minValue + " and " + maxValue);
            }
        }
        else throw new InvalidOperationException("Cannot insert value. Key must be between " + minKey + " and " + maxKey);
    }

    public void remove(Node node)
    {
        Node prev = node.prev;
        Node next = node.next;
        prev.next = next;
        next.prev = prev;
        int key = delete(node.key);
    }

    public int delete(int key){
        if (cache.ContainsKey(key)) {
            int value = cache[key].value;
            cache.Remove(key);
            return value;
        }
        else return -1;
    }

    public int get(int key){
        if (cache.ContainsKey(key)){
            Node theNode = cache[key];
            remove(theNode);
            cache[key] = new Node(key, theNode.value);
            insert(theNode);
            return theNode.value;
        }
        else return -1;
    }

    public void put(int key, int value)
    {
        if (!cache.ContainsKey(key)){
            cache[key] = new Node(key, value);
            insert(cache[key]);
            if (cache.Count() > capacity)
            {
                Node LRU = left.next;
                remove(LRU);
            }
        }
        else Console.WriteLine("The node already exists in the cache");
    }
}

public class Program{
    static void Main(string[] args)
    {
        try
        {
            LRUCache cache = new LRUCache(1001);
            cache.put(1, 1);
            cache.put(2, 2);
            Console.WriteLine(cache.get(1));
            cache.put(3, 3);    // evicts key 2
            Console.WriteLine(cache.get(2));
            cache.put(4, 4);    // evicts key 1
            Console.WriteLine(cache.get(1));
            Console.WriteLine(cache.get(3));       // returns 3
            Console.WriteLine(cache.get(4));       // returns 4
            Console.WriteLine(cache.delete(3));    // returns 3
            Console.WriteLine(cache.get(3));       // returns -1 (not found)
        }
        catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }   
}