const p1 = [21.0255, 52.21915];
const p2 = [21.04563, 52.21918];
const p3 = [21.04726, 52.21163];
const p4 = [21.02668, 52.21172];

db.places.find({
    location: {
        $geoWithin: {
            $geometry: {
                type: "Polygon",
                coordinates: [[p1, p2, p3, p4, p1]]
            }
        }
    }
})