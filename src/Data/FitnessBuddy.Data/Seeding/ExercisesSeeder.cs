namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;

    public class ExercisesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Exercises.Any())
            {
                return;
            }

            var adminId = dbContext.Users
                .Where(x => x.Email == "admin@admin.bg")
                .Select(x => x.Id)
                .FirstOrDefault();

            var exercises = new List<Exercise>
            {
                // Shoulders
                new Exercise
                {
                    Name = "Arnold Dumbbell Press",
                    Description = @$"• Sit on an exercise bench with back support and hold two dumbbells in front of you at about upper chest level with your palms facing your body and your elbows bent.
• Tip: Your arms should be next to your torso. The starting position should look like the contracted portion of a dumbbell curl.
• Now to perform the movement, raise the dumbbells as you rotate the palms of your hands until they are facing forward.
• Continue lifting the dumbbells until your arms are extended above you in straight arm position. Breathe out as you perform this portion of the movement.
• After a second pause at the top, begin to lower the dumbbells to the original position by rotating the palms of your hands towards you. 
• Tip: The left arm will be rotated in a counter clockwise manner while the right one will be rotated clockwise. Breathe in as you perform this portion of the movement.
• Repeat for the recommended amount of repetitions.
• Variations: You can perform the exercise standing up but that is not recommended for people with lower back issues.",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Shoulders")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/shoulder1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/X60-yTMOJfw",
                },
                new Exercise
                {
                    Name = "Seated Barbell Military Press",
                    Description = @"Sit on a Military Press Bench with a bar behind your head and either have a spotter give you the bar (better on the rotator cuff this way) or pick it up yourself carefully with a pronated grip (palms facing forward). Tip: Your grip should be wider than shoulder width and it should create a 90-degree angle between the forearm and the upper arm as the barbell goes down.
Once you pick up the barbell with the correct grip length, lift the bar up over your head by locking your arms. Hold at about shoulder level and slightly in front of your head. This is your starting position.
Lower the bar down to the collarbone slowly as you inhale.
Lift the bar back up to the starting position as you exhale.
Repeat for the recommended amount of repetitions.",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Shoulders")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/shoulder2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/lPFwcHl0a2c",
                },
                new Exercise
                {
                    Name = "Military Press",
                    Description = @"Start by placing a barbell that is about chest high on a squat rack. Once you have selected the weights, grab the barbell using a pronated (palms facing forward) grip. Make sure to grip the bar wider than shoulder width apart from each other.
Slightly bend the knees and place the barbell on your collar bone. Lift the barbell up keeping it lying on your chest. Take a step back and position your feet shoulder width apart from each other.
Once you pick up the barbell with the correct grip length, lift the bar up over your head by locking your arms. Hold at about shoulder level and slightly in front of your head. This is your starting position.
Lower the bar down to the collarbone slowly as you inhale.
Lift the bar back up to the starting position as you exhale.
Repeat for the recommended amount of repetitions.",
                    Difficulty = ExerciseDifficulty.Expert,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Shoulders")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/shoulder3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/8E4oWpi0RkA",
                },
                new Exercise
                {
                    Name = "Single-arm Barbell Lateral Raise",
                    Description = @"• Hold onto an object that's secured like a power cage, squat rack, or cable stand. 
• Be sure to keep your body still so that you don't start swinging to get the weight up, which would reduce the isolation effect. 
• I use this variation mostly as a finishing exercise at the end of my shoulder workout.",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Shoulders")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/shoulder4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/Ba-HmYol59U",
                },

                // Triceps
                new Exercise
                {
                    Name = "Wide grip skull crushers",
                    Description = @"To perform this exercise, lay back on the bench with your arms perpendicular to your chest. Lower the barbell to your forehead and extend back up using your triceps to move the weight. You can increase the intensity and focus on the long head of the triceps by moving your arm position, extending the arms farther over the head, allowing the bar to go above your head. With the arms extended farther back it allows for consistent gravity throughout the full range of the motion. The farther you extend your arms the more stretching that is applied on the long head resulting on a movement more focused on the long head of the triceps which provides the most mass to the back of the arm",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Triceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "EZ Curl Bar")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/triceps1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/4re6CJ0XNF8",
                },
                new Exercise
                {
                    Name = "Triceps Pushdown",
                    Description = @"Attach a straight or angled bar to a high pulley and grab with an overhand grip (palms facing down) at shoulder width.
Standing upright with the torso straight and a very small inclination forward, bring the upper arms close to your body and perpendicular to the floor. The forearms should be pointing up towards the pulley as they hold the bar. This is your starting position.
Using the triceps, bring the bar down until it touches the front of your thighs and the arms are fully extended perpendicular to the floor. The upper arms should always remain stationary next to your torso and only the forearms should move. Exhale as you perform this movement.
After a second hold at the contracted position, bring the bar slowly up to the starting point. Breathe in as you perform this step.
Repeat for the recommended amount of repetitions.
Variations: There are many variations to this movement. For instance you can use an E-Z bar attachment as well as a V-angled bar that allows the thumb to be higher than the small finger. Also, you can attach a rope to the pulley as well as using a reverse grip on the bar exercises.",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Triceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/triceps2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/HIKzvHkibWc",
                },
                new Exercise
                {
                    Name = "Tricep Dumbbell Kickback",
                    Description = @"Start with a dumbbell in each hand and your palms facing your torso. Keep your back straight with a slight bend in the knees and bend forward at the waist. Your torso should be almost parallel to the floor. Make sure to keep your head up. Your upper arms should be close to your torso and parallel to the floor. Your forearms should be pointed towards the floor as you hold the weights. There should be a 90-degree angle formed between your forearm and upper arm. This is your starting position.
Now, while keeping your upper arms stationary, exhale and use your triceps to lift the weights until the arm is fully extended. Focus on moving the forearm.
After a brief pause at the top contraction, inhale and slowly lower the dumbbells back down to the starting position.
Repeat the movement for the prescribed amount of repetitions.
Variations: This exercise can be executed also one arm at a time much like the one arm rows are performed.",
                    Difficulty = ExerciseDifficulty.Expert,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Triceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/triceps3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/HyqTb_jE_oI",
                },
                new Exercise
                {
                    Name = "Dumbbell Triceps Extension",
                    Description = @"To begin, stand up with a dumbbell held by both hands. Your feet should be about shoulder width apart from each other. Slowly use both hands to grab the dumbbell and lift it over your head until both arms are fully extended.
The resistance should be resting in the palms of your hands with your thumbs around it. The palm of the hands should be facing up towards the ceiling. This will be your starting position.
Keeping your upper arms close to your head with elbows in and perpendicular to the floor, lower the resistance in a semicircular motion behind your head until your forearms touch your biceps. Tip: The upper arms should remain stationary and only the forearms should move. Breathe in as you perform this step.
Go back to the starting position by using the triceps to raise the dumbbell. Breathe out as you perform this step.
Repeat for the recommended amount of repetitions",
                    Difficulty = ExerciseDifficulty.Expert,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Triceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/triceps4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/ntBjdnckWgo",
                },
                new Exercise
                {
                    Name = "Cable Rope Overhead Tricep Extension",
                    Description = @"Attach a rope to the bottom pulley of the pulley machine.
Grasping the rope with both hands, extend your arms with your hands directly above your head using a neutral grip (palms facing each other). Your elbows should be in close to your head and the arms should be perpendicular to the floor with the knuckles aimed at the ceiling. This will be your starting position.
Slowly lower the rope behind your head as you hold the upper arms stationary. Inhale as you perform this movement and pause when your triceps are fully stretched.
Return to the starting position by flexing your triceps as you breathe out.
Repeat for the recommended amount of repetitions.
Variations: You can also do this seated with a bench that has back support, or you can use a dumbbell instead of the rope",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Triceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/triceps5.jpg",
                    VideoUrl = "https://www.youtube.com/embed/mRozZKkGIfg",
                },

                // Biceps
                new Exercise
                {
                    Name = "Barbell Curl",
                    Description = @"Stand up with your torso upright while holding a barbell at a shoulder-width grip. The palm of your hands should be facing forward and the elbows should be close to the torso. This will be your starting position.
While holding the upper arms stationary, curl the weights forward while contracting the biceps as you breathe out. Tip: Only the forearms should move.
Continue the movement until your biceps are fully contracted and the bar is at shoulder level. Hold the contracted position for a second and squeeze the biceps hard.
Slowly begin to bring the bar back to starting position as your breathe in.
Repeat for the recommended amount of repetitions",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Biceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/biceps1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/dDI8ClxRS04",
                },
                new Exercise
                {
                    Name = "Cable Hammer Curl",
                    Description = @"Grasp the rope with a neutral (palms-in) grip and stand straight up keeping the natural arch of the back and your torso stationary. Put your elbows in by your side and keep them there stationary during the entire movement. Tip: Only the forearms should move; not your upper arms",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Biceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/biceps2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/vsarApmqJmo",
                },
                new Exercise
                {
                    Name = "Preacher Curl",
                    Description = @"To perform this movement you will need a preacher bench and an E-Z bar. Grab the E-Z curl bar at the close inner handle (either have someone hand you the bar which is preferable or grab the bar from the front bar rest provided by most preacher benches). The palm of your hands should be facing forward and they should be slightly tilted inwards due to the shape of the bar.
With the upper arms positioned against the preacher bench pad and the chest against it, hold the E-Z Curl Bar at shoulder length. This will be your starting position.
As you breathe in, slowly lower the bar until your upper arm is extended and the biceps is fully stretched.
As you exhale, use the biceps to curl the weight up until your biceps is fully contracted and the bar is at shoulder height. Squeeze the biceps hard and hold this position for a second",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Biceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/biceps3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/RgN216Cumtw",
                },
                new Exercise
                {
                    Name = "Dumbbell Bicep Curl",
                    Description = @"Stand up straight with a dumbbell in each hand at arm's length. Keep your elbows close to your torso and rotate the palms of your hands until they are facing forward. This will be your starting position.
Now, keeping the upper arms stationary, exhale and curl the weights while contracting your biceps. Continue to raise the weights until your biceps are fully contracted and the dumbbells are at shoulder level. Hold the contracted position for a brief pause as you squeeze your biceps.
Then, inhale and slowly begin to lower the dumbbells back to the starting position.
Repeat for the recommended amount of repetitions",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Biceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/biceps4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/3OZ2MT_5r3Q",
                },
                new Exercise
                {
                    Name = "Concentration Curls",
                    Description = @"Sit down on a flat bench with one dumbbell in front of you between your legs. Your legs should be spread with your knees bent and feet on the floor.
Use your right arm to pick the dumbbell up. Place the back of your right upper arm on the top of your inner right thigh. Rotate the palm of your hand until it is facing forward away from your thigh. Tip: Your arm should be extended and the dumbbell should be above the floor. This will be your starting position.
While holding the upper arm stationary, curl the weights forward while contracting the biceps as you breathe out. Only the forearms should move. Continue the movement until your biceps are fully contracted and the dumbbells are at shoulder level. Tip: At the top of the movement make sure that the little finger of your arm is higher than your thumb. This guarantees a good contraction. Hold the contracted position for a second as you squeeze the biceps.
Slowly begin to bring the dumbbells back to starting position as your breathe in. Caution: Avoid swinging motions at any time",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Biceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/biceps5.jpg",
                    VideoUrl = "https://www.youtube.com/embed/ZcU2hN76UyA",
                },

                // Back
                new Exercise
                {
                    Name = "Straight Arm Pulldown",
                    Description = @"You will start by grabbing the wide bar from the top pulley of a pulldown machine and using a wider than shoulder-width pronated (palms down) grip. Step backwards two feet or so.
Bend your torso forward at the waist by around 30-degrees with your arms fully extended in front of you and a slight bend at the elbows. If your arms are not fully extended then you need to step a bit more backwards until they are. Once your arms are fully extended and your torso is slightly bent at the waist, tighten the lats and then you are ready to begin.
While keeping the arms straight, pull the bar down by contracting the lats until your hands are next to the side of the thighs. Breathe out as you perform this step.
While keeping the arms straight, go back to the starting position while breathing in",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Back")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/back1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/wcVDItawocI",
                },
                new Exercise
                {
                    Name = "Rack Pulls",
                    Description = @"Set up in a power rack with the bar on the pins. The pins should be set to the desired point; just below the knees, just above, or in the mid thigh position. Position yourself against the bar in proper deadlifting position. Your feet should be under your hips, your grip shoulder width, back arched, and hips back to engage the hamstrings. Since the weight is typically heavy, you may use a mixed grip, a hook grip, or use straps to aid in holding the weight.
With your head looking forward, extend through the hips and knees, pulling the weight up and back until lockout. Be sure to pull your shoulders back as you complete the movement.
Return the weight to the pins and repeat",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Back")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/back2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/u7NE34Vw81w",
                },
                new Exercise
                {
                    Name = "Straight-Arm Pulldown",
                    Description = @"You will start by grabbing the wide bar from the top pulley of a pulldown machine and using a wider than shoulder-width pronated (palms down) grip. Step backwards two feet or so.
Bend your torso forward at the waist by around 30-degrees with your arms fully extended in front of you and a slight bend at the elbows. If your arms are not fully extended then you need to step a bit more backwards until they are. Once your arms are fully extended and your torso is slightly bent at the waist, tighten the lats and then you are ready to begin.
While keeping the arms straight, pull the bar down by contracting the lats until your hands are next to the side of the thighs. Breathe out as you perform this step.
While keeping the arms straight, go back to the starting position while breathing in",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Back")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/back3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/r34PR1mxzmU",
                },
                new Exercise
                {
                    Name = "Bent Over Barbell Row",
                    Description = @"Holding a barbell with a pronated grip (palms facing down), bend your knees slightly and bring your torso forward, by bending at the waist, while keeping the back straight until it is almost parallel to the floor. Tip: Make sure that you keep the head up. The barbell should hang directly in front of you as your arms hang perpendicular to the floor and your torso. This is your starting position.
Now, while keeping the torso stationary, breathe out and lift the barbell to you. Keep the elbows close to the body and only use the forearms to hold the weight. At the top contracted position, squeeze the back muscles and hold for a brief pause.
Then inhale and slowly lower the barbell back to the starting position.
Repeat for the recommended amount of repetitions.
Caution: This exercise is not recommended for people with back problems. A Low Pulley Row is a better choice for people with back issues.
Also, just like with the bent knee dead-lift, if you have a healthy back, ensure perfect form and never slouch the back forward as this can cause back injury.
Be cautious as well with the weight used; in case of doubt, use less weight rather than more",
                    Difficulty = ExerciseDifficulty.Expert,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Back")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/back4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/xlBxIMqh3A",
                },
                new Exercise
                {
                    Name = "Wide Grip Lat Pulldown",
                    Description = @"Sit down on a pull-down machine with a wide bar attached to the top pulley. Make sure that you adjust the knee pad of the machine to fit your height. These pads will prevent your body from being raised by the resistance attached to the bar.
Grab the bar with the palms facing forward using the prescribed grip. Note on grips: For a wide grip, your hands need to be spaced out at a distance wider than shoulder width. For a medium grip, your hands need to be spaced out at a distance equal to your shoulder width and for a close grip at a distance smaller than your shoulder width.
As you have both arms extended in front of you holding the bar at the chosen grip width, bring your torso back around 30 degrees or so while creating a curvature on your lower back and sticking your chest out. This is your starting position.
As you breathe out, bring the bar down until it touches your upper chest by drawing the shoulders and the upper arms down and back. Tip: Concentrate on squeezing the back muscles once you reach the full contracted position. The upper torso should remain stationary and only the arms should move. The forearms should do no other work except for holding the bar; therefore do not try to pull down the bar using the forearms.
After a second at the contracted position squeezing your shoulder blades together, slowly raise the bar back to the starting position when your arms are fully extended and the lats are fully stretched. Inhale during this portion of the movement.
Repeat this motion for the prescribed amount of repetitions.
Variations: The behind the neck variation is not recommended as it can be hard on the rotator cuff due to the hyperextension created by bringing the bar behind the neck",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Back")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/back5.jpg",
                    VideoUrl = "https://www.youtube.com/embed/S0no-Q03h74",
                },

                // Chest
                new Exercise
                {
                    Name = "Bench Press",
                    Description = @"Lie back on a flat bench. Using a medium width grip (a grip that creates a 90-degree angle in the middle of the movement between the forearms and the upper arms), lift the bar from the rack and hold it straight over you with your arms locked. This will be your starting position.
From the starting position, breathe in and begin coming down slowly until the bar touches your middle chest.
After a brief pause, push the bar back to the starting position as you breathe out. Focus on pushing the bar using your chest muscles. Lock your arms and squeeze your chest in the contracted position at the top of the motion, hold for a second and then start coming down slowly again. Tip: Ideally, lowering the weight should take about twice as long as raising it",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Chest")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/chest1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/Qjxrp9Hwv_Q",
                },
                new Exercise
                {
                    Name = "Dips Chest Version",
                    Description = @"For this exercise you will need access to parallel bars. To get yourself into the starting position, hold your body at arms length (arms locked) above the bars.
While breathing in, lower yourself slowly with your torso leaning forward around 30 degrees or so and your elbows flared out slightly until you feel a slight stretch in the chest.
Once you feel the stretch, use your chest to bring your body back to the starting position as you breathe out. Tip: Remember to squeeze the chest at the top of the movement for a second.
Repeat the movement for the prescribed amount of repetitions",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Chest")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/chest2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/4la6BkUBLgo",
                },
                new Exercise
                {
                    Name = "Dumbbell Bench Press",
                    Description = @"Lie back on a bench holding a dumbbell in each hand just to the sides of your shoulders. Your palms should be facing towards your feet in the starting position, although if you have shoulder issues then switch to a neutral grip, where the palms face each other.
Press the weights above your chest by extending your elbows until your arms are straight, then bring the weights back down slowly. To take advantage of the range of movement offered by using dumbbells rather than a barbell, take the weights down past your shoulders and bring them closer together at the top of the movement. Don’t touch them at the top, though, because that will take some of the strain off your muscles",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Chest")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/chest3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/Vc63DPUoA40",
                },
                new Exercise
                {
                    Name = "Cable Crossover",
                    Description = @"To get yourself into the starting position, place the pulleys on a high position (above your head), select the resistance to be used and hold the pulleys in each hand.
Step forward in front of an imaginary straight line between both pulleys while pulling your arms together in front of you. Your torso should have a small forward bend from the waist. This will be your starting position.
With a slight bend on your elbows in order to prevent stress at the biceps tendon, extend your arms to the side (straight out at both sides) in a wide arc until you feel a stretch on your chest. Breathe in as you perform this portion of the movement. Tip: Keep in mind that throughout the movement, the arms and torso should remain stationary; the movement should only occur at the shoulder joint.
Return your arms back to the starting position as you breathe out. Make sure to use the same arc of motion used to lower the weights.
Hold for a second at the starting position and repeat the movement for the prescribed amount of repetitions",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Chest")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/chest4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/aoP0s_MjN-g",
                },
                new Exercise
                {
                    Name = "Pushups",
                    Description = @"Lie on the floor face down and place your hands about 36 inches apart while holding your torso up at arms length.
Next, lower yourself downward until your chest almost touches the floor as you inhale.
Now breathe out and press your upper body back up to the starting position while squeezing your chest.
After a brief pause at the top contracted position, you can begin to lower yourself downward again for as many repetitions as needed",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Chest")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/chest5.jpg",
                    VideoUrl = "https://www.youtube.com/embed/XIHO5t_VBPQ",
                },

                // Forearm
                new Exercise
                {
                    Name = "Behind The Back Wrist Curl",
                    Description = @"Start by standing straight and holding a barbell behind your glutes at arm's length while using a pronated grip (palms will be facing back away from the glutes) and having your hands shoulder width apart from each other.
You should be looking straight forward while your feet are shoulder width apart from each other. This is the starting position.
While exhaling, slowly elevate the barbell up by curling your wrist in a semi-circular motion towards the ceiling. Note: Your wrist should be the only body part moving for this exercise.
Hold the contraction for a second and lower the barbell back down to the starting position while inhaling",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Forearm")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/forearm1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/sVLVLcsfWSo",
                },
                new Exercise
                {
                    Name = "Farmer's Walk",
                    Description = @"There are various implements that can be used for the farmers walk. These can also be performed with heavy dumbbells or short bars if these implements aren't available. Begin by standing between the implements.
After gripping the handles, lift them up by driving through your heels, keeping your back straight and your head up.
Walk taking short, quick steps, and don't forget to breathe. Move for a given distance, typically 50-100 feet, as fast as possible",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Forearm")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/forearm2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/hJW-Xc8TvU8",
                },
                new Exercise
                {
                    Name = "Wrist Roller",
                    Description = @"To begin, stand straight up grabbing a wrist roller using a pronated grip (palms facing down). Your feet should be shoulder width apart.
Slowly lift both arms until they are fully extended and parallel to the floor in front of you. Note: Make sure the rope is not wrapped around the roller. Your entire body should be stationary except for the forearms. This is the starting position.
Rotate one wrist at a time in an upward motion to bring the weight up to the bar by rolling the rope around the roller.
Once the weight has reached the bar, slowly begin to lower the weight back down by rotating the wrist in a downward motion until the weight reaches the starting position",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Forearm")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/forearm3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/-lOFG0U_rlY",
                },

                // Traps
                new Exercise
                {
                    Name = "Barbell Shrug",
                    Description = @"Stand up straight with your feet at shoulder width as you hold a barbell with both hands in front of you using a pronated grip (palms facing the thighs). Tip: Your hands should be a little wider than shoulder width apart. You can use wrist wraps for this exercise for a better grip. This will be your starting position.
Raise your shoulders up as far as you can go as you breathe out and hold the contraction for a second. Tip: Refrain from trying to lift the barbell by using your biceps.
Slowly return to the starting position as you breathe in.
Repeat for the recommended amount of repetitions.
Variations: You can also rotate your shoulders as you go up, going in a semicircular motion from front to rear. However this version is not good for people with shoulder problems. In addition, this exercise can be performed with the barbell behind the back, with dumbbells by the side, a smith machine or with a shrug machine",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Traps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/traps1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/9xGqgGFAtiM",
                },
                new Exercise
                {
                    Name = "Dumbbell Shrug",
                    Description = @"Stand erect with a dumbbell on each hand (palms facing your torso), arms extended on the sides.
Lift the dumbbells by elevating the shoulders as high as possible while you exhale. Hold the contraction at the top for a second. Tip: The arms should remain extended at all times. Refrain from using the biceps to help lift the dumbbells. Only the shoulders should be moving up and down.
Lower the dumbbells back to the original position",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Traps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/traps2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/8lP_eJvClSA",
                },

                // Abs
                new Exercise
                {
                    Name = "Crunches",
                    Description = @"Lie down on your back. Plant your feet on the floor, hip-width apart. Bend your knees and place your arms across your chest. Contract your abs and inhale.
Exhale and lift your upper body, keeping your head and neck relaxed.
Inhale and return to the starting position",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Abs")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/abs1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/YdZakh0Pkwc",
                },
                new Exercise
                {
                    Name = "Cable Crunch",
                    Description = @"Kneel below a high pulley that contains a rope attachment.
Grasp cable rope attachment and lower the rope until your hands are placed next to your face.
Flex your hips slightly and allow the weight to hyperextend the lower back. This will be your starting position.
With the hips stationary, flex the waist as you contract the abs so that the elbows travel towards the middle of the thighs. Exhale as you perform this portion of the movement and hold the contraction for a second.
Slowly return to the starting position as you inhale. Tip: Make sure that you keep constant tension on the abs throughout the movement. Also, do not choose a weight so heavy that the lower back handles the brunt of the work",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Abs")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/abs2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/3qjoXDTuyOE",
                },
                new Exercise
                {
                    Name = "Reverse Crunch",
                    Description = @"Lie down on the floor with your legs fully extended and arms to the side of your torso with the palms on the floor. Your arms should be stationary for the entire exercise.
Move your legs up so that your thighs are perpendicular to the floor and feet are together and parallel to the floor. This is the starting position.
While inhaling, move your legs towards the torso as you roll your pelvis backwards and you raise your hips off the floor. At the end of this movement your knees will be touching your chest.
Hold the contraction for a second and move your legs back to the starting position while exhaling",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Abs")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/abs3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/lmSP-c1X_iY",
                },
                new Exercise
                {
                    Name = "Plank / Planking",
                    Description = @"Get into a push up position, with your elbows under your shoulders and your feet hip-width apart.
Bend your elbows and rest your weight on your forearms and on your toes, keeping your body in a straight line.
Hold for as long as possible. Proper Form And Breathing Pattern",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Abs")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/abs4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/tgbrMdfuGJA",
                },
                new Exercise
                {
                    Name = "Hanging Leg Raise",
                    Description = @"Hang from a chin-up bar with both arms extended at arms length in top of you using either a wide grip or a medium grip. The legs should be straight down with the pelvis rolled slightly backwards. This will be your starting position.
Raise your legs until the torso makes a 90-degree angle with the legs. Exhale as you perform this movement and hold the contraction for a second or so.
Go back slowly to the starting position as you breathe in",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Abs")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/abs5.jpg",
                    VideoUrl = "https://www.youtube.com/embed/Nw0LOKe3_l8",
                },

                // Glutes
                new Exercise
                {
                    Name = "Barbell Hip Thrust",
                    Description = @"Begin seated on the ground with a bench directly behind you. Have a loaded barbell over your legs. Using a fat bar or having a pad on the bar can greatly reduce the discomfort caused by this exercise.
Roll the bar so that it is directly above your hips, and lean back against the bench so that your shoulder blades are near the top of it.
Begin the movement by driving through your feet, extending your hips vertically through the bar. Your weight should be supported by your shoulder blades and your feet. Extend as far as possible, then reverse the motion to return to the starting position",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Glutes")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/glutes1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/Fk1OfkMmVt4",
                },
                new Exercise
                {
                    Name = "Butt Lift Bridge",
                    Description = @"Lie flat on the floor on your back with the hands by your side and your knees bent. Your feet should be placed around shoulder width. This will be your starting position.
Pushing mainly with your heels, lift your hips off the floor while keeping your back straight. Breathe out as you perform this part of the motion and hold at the top for a second.
Slowly go back to the starting position as you breathe in",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Glutes")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/glutes2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/MqQlfjQOmHA",
                },
                new Exercise
                {
                    Name = "Glute Ham Raise",
                    Description = @"Push your toes into the foot plate as you raise your body back up until you're at a 90 degree angle. Use the hamstrings to help pull you back up. Maximize contraction at the top by squeezing your glutes and hamstrings. Maintain control as you lower yourself back down to repeat your reps",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Glutes")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Machine")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/glutes3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/TDdV0dCsqKs",
                },
                new Exercise
                {
                    Name = "Stiff Legged Dumbbell Deadlift",
                    Description = @"Grasp a couple of dumbbells holding them by your side at arm's length.
Stand with your torso straight and your legs spaced using a shoulder width or narrower stance. The knees should be slightly bent. This is your starting position.
Keeping the knees stationary, lower the dumbbells to over the top of your feet by bending at the waist while keeping your back straight. Keep moving forward as if you were going to pick something from the floor until you feel a stretch on the hamstrings. Exhale as you perform this movement
Start bringing your torso up straight again by extending your hips and waist until you are back at the starting position. Inhale as you perform this movement",
                    Difficulty = ExerciseDifficulty.Expert,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Glutes")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/glutes4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/w9_PudlkeLI",
                },

                // Quadriceps
                new Exercise
                {
                    Name = "Split Squat",
                    Description = @"Position yourself into a staggered stance with the rear foot elevated and front foot forward.
Hold a dumbbell in each hand, letting them hang at the sides. This will be your starting position.
Begin by descending, flexing your knee and hip to lower your body down. Maintain good posture througout the movement. Keep the front knee in line with the foot as you perform the exercise.
At the bottom of the movement, drive through the heel to extend the knee and hip to return to the starting position",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Quadriceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/quardiceps1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/UJWLxHAYxx4",
                },
                new Exercise
                {
                    Name = "Dumbbell Step Ups",
                    Description = @"Stand up straight while holding a dumbbell on each hand (palms facing the side of your legs).
Place the right foot on the elevated platform. Step on the platform by extending the hip and the knee of your right leg. Use the heel mainly to lift the rest of your body up and place the foot of the left leg on the platform as well. Breathe out as you execute the force required to come up.
Step down with the left leg by flexing the hip and knee of the right leg as you inhale. Return to the original standing position by placing the right foot of to next to the left foot on the initial position.
Repeat with the right leg for the recommended amount of repetitions and then perform with the left leg.
Note: This is a great exercise for people with lower back problems that are unable to do stiff legged deadlifts",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Quadriceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/quardiceps2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/7AtIjR-QqVA",
                },
                new Exercise
                {
                    Name = "Hack Squat",
                    Description = @"Position your feet at shoulder width, extend your legs, and release the safety handles. Slowly lower the weight by bending your knees until your thighs are approximately at 90 degrees. Reverse the movement by driving into the platform and extending the knees and hips",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Quadriceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Machine")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/quardiceps3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/plv5ur26Q7A",
                },
                new Exercise
                {
                    Name = "Squat",
                    Description = @"This exercise is best performed inside a squat rack for safety purposes. To begin, first set the bar on a rack to just below shoulder level. Once the correct height is chosen and the bar is loaded, step under the bar and place the back of your shoulders (slightly below the neck) across it.
Hold on to the bar using both arms at each side and lift it off the rack by first pushing with your legs and at the same time straightening your torso.
Step away from the rack and position your legs using a shoulder width medium stance with the toes slightly pointed out. Keep your head up at all times and also maintain a straight back. This will be your starting position. (Note: For the purposes of this discussion we will use the medium stance described above which targets overall development; however you can choose any of the three stances discussed in the foot stances section).
Begin to slowly lower the bar by bending the knees and hips as you maintain a straight posture with the head up. Continue down until the angle between the upper leg and the calves becomes slightly less than 90-degrees. Inhale as you perform this portion of the movement. Tip: If you performed the exercise correctly, the front of the knees should make an imaginary straight line with the toes that is perpendicular to the front. If your knees are past that imaginary line (if they are past your toes) then you are placing undue stress on the knee and the exercise has been performed incorrectly.
Begin to raise the bar as you exhale by pushing the floor with the heel of your foot as you straighten the legs again and go back to the starting position",
                    Difficulty = ExerciseDifficulty.Expert,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Quadriceps")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Barbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/quardiceps4.jpg",
                    VideoUrl = "https://www.youtube.com/embed/tVB1q8zkP3o",
                },

                // Hamstrings
                new Exercise
                {
                    Name = "Lying Leg Curls",
                    Description = @"djust the machine lever to fit your height and lie face down on the leg curl machine with the pad of the lever on the back of your legs (just a few inches under the calves). Tip: Preferably use a leg curl machine that is angled as opposed to flat since an angled position is more favorable for hamstrings recruitment.
Keeping the torso flat on the bench, ensure your legs are fully stretched and grab the side handles of the machine. Position your toes straight (or you can also use any of the other two stances described on the foot positioning section). This will be your starting position.
As you exhale, curl your legs up as far as possible without lifting the upper legs from the pad. Once you hit the fully contracted position, hold it for a second.
As you inhale, bring the legs back to the initial position",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Hamstrings")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Machine")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/hamstrings1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/jxctD6fL_FQ",
                },
                new Exercise
                {
                    Name = "One-Legged Cable Kickback",
                    Description = @"Hook a leather ankle cuff to a low cable pulley and then attach the cuff to your ankle.
Face the weight stack from a distance of about two feet, grasping the steel frame for support.
While keeping your knees and hips bent slightly and your abs tight, contract your glutes to slowly 'kick' the working leg back in a semicircular arc as high as it will comfortably go as you breathe out. Tip: At full extension, squeeze your glutes for a second in order to achieve a peak contraction.
Now slowly bring your working leg forward, resisting the pull of the cable until you reach the starting position",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Hamstrings")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Cable")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/hamstrings2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/xO5WVJGVJ2w",
                },
                new Exercise
                {
                    Name = "Stability Ball Pike with Knee Tuck",
                    Description = @"1. Get in the push-up position with your hands shoulder-width apart, securing a large stability ball under your lower quads. Your body should be almost straight with your feet together off the floor. This is your start position.
2. Contract your lower abs to raise your hips as high as you can, allowing the ball to roll down to your shins under control and bringing your knees into your chest.
3. Push your knees back out and allow your abs to relax to roll the ball back in the reverse direction.
4.Repeat for the required number of reps",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Hamstrings")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Exercise Ball")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/hamstrings3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/BQsUYSGfJq4",
                },

                // Calves
                new Exercise
                {
                    Name = "Standing Calf Raises",
                    Description = @"Adjust the padded lever of the calf raise machine to fit your height.
Place your shoulders under the pads provided and position your toes facing forward (or using any of the two other positions described at the beginning of the chapter). The balls of your feet should be secured on top of the calf block with the heels extending off it. Push the lever up by extending your hips and knees until your torso is standing erect. The knees should be kept with a slight bend; never locked. Toes should be facing forward, outwards or inwards as described at the beginning of the chapter. This will be your starting position.
Raise your heels as you breathe out by extending your ankles as high as possible and flexing your calf. Ensure that the knee is kept stationary at all times. There should be no bending at any time. Hold the contracted position by a second before you start to go back down.
Go back slowly to the starting position as you breathe in by lowering your heels as you bend the ankles until calves are stretched",
                    Difficulty = ExerciseDifficulty.Beginner,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Calves")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Machine")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/calves1.jpg",
                    VideoUrl = "https://www.youtube.com/embed/MAMzF7iZNkc",
                },
                new Exercise
                {
                    Name = "Standing Dumbbell Calf Raises",
                    Description = @"Stand with your torso upright holding two dumbbells in your hands by your sides. Place the ball of the foot on a sturdy and stable wooden board (that is around 2-3 inches tall) while your heels extend off and touch the floor. This will be your starting position.
With the toes pointing either straight (to hit all parts equally), inwards (for emphasis on the outer head) or outwards (for emphasis on the inner head), raise the heels off the floor as you exhale by contracting the calves. Hold the top contraction for a second.
As you inhale, go back to the starting position by slowly lowering the heels",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Calves")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Dumbbell")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/calves2.jpg",
                    VideoUrl = "https://www.youtube.com/embed/wxwY7GXxL4k",
                },
                new Exercise
                {
                    Name = "Donkey Calf Raises",
                    Description = @"For this exercise you will need access to a donkey calf raise machine. Start by positioning your lower back and hips under the padded lever provided. The tailbone area should be the one making contact with the pad.
Place both of your arms on the side handles and place the balls of your feet on the calf block with the heels extending off. Align the toes forward, inward or outward, depending on the area you wish to target, and straighten the knees without locking them. This will be your starting position.
Raise your heels as you breathe out by extending your ankles as high as possible and flexing your calf. Ensure that the knee is kept stationary at all times. There should be no bending at any time. Hold the contracted position by a second before you start to go back down.
Go back slowly to the starting position as you breathe in by lowering your heels as you bend the ankles until calves are stretched",
                    Difficulty = ExerciseDifficulty.Intermediate,
                    AddedByUserId = adminId,
                    CategoryId = dbContext.ExerciseCategories
                    .Where(x => x.Name == "Calves")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    EquipmentId = dbContext.ExerciseEquipment
                    .Where(x => x.Name == "Body only")
                    .Select(x => x.Id)
                    .FirstOrDefault(),
                    ImageUrl = "/images/exercises/calves3.jpg",
                    VideoUrl = "https://www.youtube.com/embed/XEZUIXn5mgc",
                },
            };

            dbContext.Exercises.AddRange(exercises);

            await dbContext.SaveChangesAsync();
        }
    }
}
