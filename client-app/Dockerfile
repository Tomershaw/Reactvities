# Build stage
FROM node:18 AS build

# תיקיית עבודה
WORKDIR /app

# מעתיקים את כל קבצי ה-Client
COPY . .

# מתקינים את כל התלויות
RUN npm install

# מתקינים גם את TypeScript כדי שהפקודת build תרוץ
RUN npm install typescript

# בונים את האפליקציה עם קובץ Production נפרד
RUN npm run build -- --config vite.config.prod.ts

# Serve stage (בעזרת Nginx)
FROM nginx:alpine

# מעתיקים את התוצר של הבנייה אל תוך השרת
COPY --from=build /app/dist /usr/share/nginx/html

# פותחים את פורט 80
EXPOSE 80

# מריצים את nginx
CMD ["nginx", "-g", "daemon off;"]
