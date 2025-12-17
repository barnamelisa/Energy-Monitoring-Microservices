package org.example.messaging;

import org.example.config.RabbitConfig;
import org.example.services.UserSyncMemoryService;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Service;

import java.util.Map;

@Service
public class UserSyncListener {

    private final UserSyncMemoryService memoryService;

    public UserSyncListener(UserSyncMemoryService memoryService) {
        this.memoryService = memoryService;
    }

    @RabbitListener(queues = RabbitConfig.SYNC_QUEUE)
    public void handleUserCreated(Map<String, Object> message) {

        System.out.println("📥 [DeviceService] Received RabbitMQ message: " + message);

        if (!"USER_CREATED".equals(message.get("event"))) return;

        String userId = message.get("userId").toString();

        // store in memory
        memoryService.addUser(userId);

        System.out.println("🔄 [DeviceService] Synced user: " + userId);
    }
}
