var muscleMap = document.getElementById("muscle-map");

muscleMap.addEventListener("click", (event) => {
    var muscleId = event.target.id;
    var url = `${window.location}Exercises/Category/`;

    if (muscleId.includes("traps")) {
        url += "Traps"
    } else if (muscleId.includes("shoulders")) {
        url += "Shoulders"
    } else if (muscleId.includes("chest")) {
        url += "Chest"
    } else if (muscleId.includes("biceps")) {
        url += "Biceps"
    } else if (muscleId.includes("forearm")) {
        url += "Forearm"
    } else if (muscleId.includes("abs")) {
        url += "Abs"
    } else if (muscleId.includes("quads")) {
        url += "Quadriceps"
    } else if (muscleId.includes("calves")) {
        url += "Calves"
    } else if (muscleId.includes("triceps")) {
        url += "Triceps"
    } else if (muscleId.includes("lower")) {
        url += "Lower body"
    } else if (muscleId.includes("glutes")) {
        url += "Glutes"
    } else if (muscleId.includes("hamstrings")) {
        url += "Hamstrings"
    } else if (muscleId.includes("lats")) {
        url += "Back"
    }

    window.location = url;
})