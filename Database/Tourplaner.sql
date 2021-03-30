CREATE TABLE "Tour" (
  "Tour_ID" SERIAL PRIMARY KEY,
  "Name" varchar(50) UNIQUE NOT NULL,
  "Description" varchar(2000),
  "From" varchar(50),
  "To" varchar(50),
  "RouteType" int NOT NULL
);

CREATE TABLE "TourLog" (
  "TourLog_ID" SERIAL PRIMARY KEY,
  "Tour_ID" int NOT NULL,
  "TourDate" timestamp NOT NULL,
  "Distance" decimal NOT NULL,
  "AvgSpeed" decimal NOT NULL,
  "Breaks" int NOT NULL,
  "Brawls" int NOT NULL,
  "Abductions" int NOT NULL,
  "HobgoblinSightings" int NOT NULL,
  "UFOSightings" int NOT NULL,
  "TotalTime" decimal NOT NULL,
  "rating" int NOT NULL
);

ALTER TABLE "TourLog" ADD FOREIGN KEY ("Tour_ID") REFERENCES "Tour" ("Tour_ID");
