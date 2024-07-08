namespace Naukri.Moltk.Fusion
{
    public class Subscription
    {
        private readonly Node input;

        private readonly Node output;

        internal Subscription(Node input, Node output)
        {
            this.input = input;
            this.output = output;
        }

        public void Start()
        {
            Node.Subscribe(input, output);
        }

        public void Cancel()
        {
            Node.Unsubscribe(input, output);
        }
    }
}
