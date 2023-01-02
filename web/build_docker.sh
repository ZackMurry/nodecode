cd backend
./gradlew build -x test
sudo docker build --build-arg JAR_FILE=build/libs/nodecode-backend-0.0.1.jar -t zackmurry/nodecode-backend .

cd ../frontend
sudo docker build -t zackmurry/nodecode-frontend .

