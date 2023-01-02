cd nginx
sudo nginx -s quit
sudo nginx

cd ..
sudo docker-compose --env-file .env up -d

