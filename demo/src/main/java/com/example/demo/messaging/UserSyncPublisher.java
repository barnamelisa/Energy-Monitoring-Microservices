package com.example.demo.messaging;

import com.example.demo.config.RabbitConfig;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

@Service
public class UserSyncPublisher {

    private final RabbitTemplate rabbitTemplate;

    public UserSyncPublisher(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    // trimitem un mesaj JSON la crearea user-ului
    public void publishUserCreated(UUID id, String username) {
        Map<String, Object> message = new HashMap<>();
        message.put("event", "USER_CREATED");
        message.put("userId", id.toString());   // trimitem UUID ca string
        message.put("username", username);

        rabbitTemplate.convertAndSend(RabbitConfig.SYNC_QUEUE, message);
        System.out.println("📤 [UserService] Sent USER_CREATED event to queue: " + message);
    }

}
