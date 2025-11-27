FROM nginx:alpine

# Xóa file default của Nginx
RUN rm -rf /usr/share/nginx/html/*

# Copy dist vào Nginx web root
COPY ./dist/workmanagement-client /usr/share/nginx/html

# Expose port
EXPOSE 80

# Chạy Nginx
CMD ["nginx", "-g", "daemon off;"]
