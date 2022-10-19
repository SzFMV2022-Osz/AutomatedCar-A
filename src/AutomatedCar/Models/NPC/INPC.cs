namespace AutomatedCar.Models.NPC
{
    using global::AutomatedCar.SystemComponents;

    public interface INPC
    {
        MoveComponent MoveComponent { get; set; }

        void Move();
    }
}
