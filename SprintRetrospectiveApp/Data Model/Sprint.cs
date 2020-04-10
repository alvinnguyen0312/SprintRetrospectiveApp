/**Purpose: this class models data related to Sprint
 * 
 */
namespace SprintRetrospectiveApp.Models
{
    public class Sprint
    {
        public int Id { get; set; }

        public double Velocity { get; set; }

        public Sprint(int Id, double Velocity)
        {
            this.Id = Id;
            this.Velocity = Velocity;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Velocity: {Velocity}";
        }
    }
}
