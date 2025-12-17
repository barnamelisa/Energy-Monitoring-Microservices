package org.example.messaging;

import org.example.config.RabbitConfig;
import org.example.entities.Device;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.Map;

@Service   // 👈 OBLIGATORIU! altfel nu se poate autowired
public class DeviceSyncPublisher {

    private final RabbitTemplate rabbitTemplate;

    public DeviceSyncPublisher(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    public void sendDeviceCreatedEvent(Device device) {

        Map<String, Object> message = new HashMap<>();
        message.put("event", "DEVICE_CREATED");
        message.put("deviceId", device.getId().toString());
        message.put("userId", device.getUserId());
        message.put("maxConsumption", device.getMaximumConsumptionValue());

        System.out.println("📤 [DeviceService] Sent DEVICE_CREATED event to queue: " + message);

        rabbitTemplate.convertAndSend(RabbitConfig.DEVICE_SYNC_QUEUE, message);
    }
}
