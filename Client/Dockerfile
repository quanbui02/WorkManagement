FROM nginx:alpine

# Xóa file default của Nginx
RUN rm -rf /usr/share/nginx/html/*

# Copy FE build vào
COPY ./dist/browser /usr/share/nginx/html

# Thay default.conf của Nginx bằng file SPA fallback
RUN printf "server {\n\
    listen 80;\n\
    root /usr/share/nginx/html;\n\
    index index.html;\n\
    location / {\n\
        try_files \$uri \$uri/ /index.html;\n\
    }\n\
}\n" > /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
