package org.example.services;

import org.example.dtos.DeviceDTO;
import org.example.dtos.DeviceDetailsDTO;
import org.example.dtos.builders.DeviceBuilder;
import org.example.entities.Device;
import org.example.handlers.exceptions.model.ResourceNotFoundException;
import org.example.messaging.DeviceSyncPublisher;
import org.example.repositories.DeviceRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

@Service
public class DeviceService {

    private static final Logger LOGGER = LoggerFactory.getLogger(DeviceService.class);

    private final DeviceRepository deviceRepository;
    private final DeviceSyncPublisher deviceSyncPublisher;
    private final UserSyncMemoryService memoryService;

    @Autowired
    public DeviceService(DeviceRepository deviceRepository,
                         DeviceSyncPublisher deviceSyncPublisher,
                         UserSyncMemoryService memoryService) {
        this.deviceRepository = deviceRepository;
        this.deviceSyncPublisher = deviceSyncPublisher;
        this.memoryService = memoryService;
    }

    public List<DeviceDTO> findDevices() {
        return deviceRepository.findAll()
                .stream()
                .map(DeviceBuilder::toDeviceDTO)
                .collect(Collectors.toList());
    }

    public List<DeviceDTO> findDevicesByUserId(String userId) {
        return deviceRepository.findByUserId(userId)
                .stream()
                .map(DeviceBuilder::toDeviceDTO)
                .collect(Collectors.toList());
    }

    public DeviceDetailsDTO findDeviceById(UUID id) {
        return deviceRepository.findById(id)
                .map(DeviceBuilder::toDeviceDetailsDTO)
                .orElseThrow(() -> {
                    LOGGER.error("Device with id {} was not found in db", id);
                    return new ResourceNotFoundException(Device.class.getSimpleName() + " with id: " + id);
                });
    }

    /**
     * INSERT DEVICE + SEND RABBITMQ MESSAGE
     */
    public UUID insert(DeviceDetailsDTO deviceDTO) {
        Device device = DeviceBuilder.toEntity(deviceDTO);
        device = deviceRepository.save(device);

        LOGGER.debug("Device with id {} was inserted in db", device.getId());

        deviceSyncPublisher.sendDeviceCreatedEvent(device);

        return device.getId();
    }

    /**
     * ASSIGN DEVICE TO USER
     */
    public void assignDeviceToUser(UUID deviceId, String userId) {

        // Validate if we know this user
        if (!memoryService.exists(userId)) {
            throw new RuntimeException("❌ User does not exist (not synced yet)!");
        }

        Device device = deviceRepository.findById(deviceId)
                .orElseThrow(() -> {
                    LOGGER.error("Device with id {} was not found in db", deviceId);
                    return new ResourceNotFoundException(Device.class.getSimpleName() + " with id: " + deviceId);
                });

        device.setUserId(userId);
        deviceRepository.save(device);

        LOGGER.debug("Device with id {} was assigned to user {}", deviceId, userId);
    }

    /**
     * UPDATE DEVICE
     */
    public void update(UUID id, DeviceDetailsDTO deviceDTO) {

        Device device = deviceRepository.findById(id)
                .orElseThrow(() -> {
                    LOGGER.error("Device with id {} was not found in db", id);
                    return new ResourceNotFoundException(Device.class.getSimpleName() + " with id: " + id);
                });

        device.setName(deviceDTO.getName());
        device.setMaximumConsumptionValue(deviceDTO.getMaximumConsumptionValue());

        if (deviceDTO.getUserId() != null) {
            device.setUserId(deviceDTO.getUserId());
        }

        deviceRepository.save(device);

        LOGGER.debug("Device with id {} was updated in db", id);
    }

    /**
     * DELETE DEVICE
     */
    public void delete(UUID id) {

        if (!deviceRepository.existsById(id)) {
            LOGGER.error("Device with id {} was not found in db", id);
            throw new ResourceNotFoundException(Device.class.getSimpleName() + " with id: " + id);
        }

        deviceRepository.deleteById(id);
        LOGGER.debug("Device with id {} was deleted from db", id);
    }
}
