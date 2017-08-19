namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Component_Filter : GameObjectFilter
    {
        public int Type;

        public Component_Filter(int type)
        {
            Type = type;
        }

        public override bool IsComponentFilter() => true;

        public bool TestComponent(Component c)
        {
            var go = c.Parent;
            return TestGameObject(go);
        }

        public new bool TestGameObject(Game_Object go)
        {
            var result = false;
            var c = go.GetComponent(Type, true);
            if (c != null)
                result = base.TestGameObject(go);
            return result;
        }
    }
}