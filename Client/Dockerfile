FROM nginx:alpine

# Xóa file default của Nginx
RUN rm -rf /usr/share/nginx/html/*

# Copy FE build vào
COPY ./dist/browser /usr/share/nginx/html

# Copy file cấu hình SPA vào container
COPY nginx/default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
