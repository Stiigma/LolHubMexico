using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.RiotAPI
{
    // Representa la estructura de la respuesta completa de la API de Timeline
    public class TimelineRiotDto
    {
        public MetadataTimelineDto metadata { get; set; } = new MetadataTimelineDto();
        public InfoTimelineDto info { get; set; } = new InfoTimelineDto();
    }

    // Metadatos de la línea de tiempo
    public class MetadataTimelineDto
    {
        public string dataVersion { get; set; } = "";
        public string matchId { get; set; } = "";
        public List<string> participants { get; set; } = new(); // Lista de PUUIDs
    }

    // Información detallada de la línea de tiempo
    public class InfoTimelineDto
    {
        public string endOfGameResult { get; set; } = ""; // Indica si el juego terminó en terminación.
        public long frameInterval { get; set; } // Intervalo de tiempo entre frames en milisegundos.
        public long gameId { get; set; }
        public List<ParticipantTimelineDto> participants { get; set; } = new();
        public List<FrameTimelineDto> frames { get; set; } = new();
    }

    // Información del participante dentro de la línea de tiempo
    public class ParticipantTimelineDto
    {
        public int participantId { get; set; }
        public string puuid { get; set; } = "";
    }

    // Un frame específico de la línea de tiempo (generalmente cada minuto)
    public class FrameTimelineDto
    {
        public List<EventTimelineDto> events { get; set; } = new();
        // Diccionario donde la clave es el participantId (como string) y el valor es ParticipantFrameDto
        public Dictionary<string, ParticipantFrameDto> participantFrames { get; set; } = new();
        public long timestamp { get; set; } // Tiempo en milisegundos desde el inicio del juego para este frame.
    }

    // Un evento específico que ocurre en la partida
    public class EventTimelineDto
    {
        public long timestamp { get; set; } // Tiempo del evento en milisegundos desde el inicio del juego.
        public long realTimestamp { get; set; } // Marca de tiempo real del evento.
        public string type { get; set; } = ""; // Tipo de evento (KILL, ITEM_PURCHASED, WARD_PLACED, etc.)

        // --- Propiedades comunes para varios tipos de eventos ---
        public int? participantId { get; set; } // ID del participante involucrado (si aplica)
        public PositionDto? position { get; set; } // Posición en el mapa (si aplica)

        // --- Propiedades específicas para eventos de ITEM ---
        public int? itemId { get; set; }
        public int? itemBefore { get; set; }
        public int? itemAfter { get; set; }
        public int? itemPurchased { get; set; }
        public int? itemSold { get; set; }
        public int? itemRemoved { get; set; }
        public int? itemUndone { get; set; }

        // --- Propiedades específicas para eventos de CHAMPION_KILL ---
        public int? killerId { get; set; }
        public int? victimId { get; set; }
        public List<int>? assists { get; set; } = new();

        // --- Propiedades específicas para eventos de BUILDING_KILL / ELITE_MONSTER_KILL ---
        public string? buildingType { get; set; }
        public string? laneType { get; set; }
        public string? towerType { get; set; }
        public string? monsterType { get; set; }
        public string? monsterSubType { get; set; }
        public int? teamId { get; set; } // Equipo que realizó la acción

        // --- Propiedades específicas para eventos de LEVEL_UP / SKILL_LEVEL_UP ---
        public string? levelUpType { get; set; }
        public int? skillSlot { get; set; }
        public int? level { get; set; }

        // --- Propiedades específicas para eventos de WARD ---
        public string? wardType { get; set; }
        public int? creatorId { get; set; }

        // Puedes añadir más propiedades según los tipos de eventos que te interesen y la documentación de Riot
    }

    // Estado de un participante en un frame específico
    public class ParticipantFrameDto
    {
        public ChampionStatsDto championStats { get; set; } = new ChampionStatsDto();
        public int currentGold { get; set; }
        public DamageStatsDto damageStats { get; set; } = new DamageStatsDto();
        public int goldPerSecond { get; set; }
        public int jungleMinionsKilled { get; set; }
        public int level { get; set; }
        public int minionsKilled { get; set; }
        public int participantId { get; set; }
        public PositionDto position { get; set; } = new PositionDto();
        public int timeEnemySpentControlled { get; set; }
        public int totalGold { get; set; }
        public int xp { get; set; }
    }

    // Estadísticas del campeón en un momento dado
    public class ChampionStatsDto
    {
        public int abilityHaste { get; set; }
        public int abilityPower { get; set; }
        public int armor { get; set; }
        public int armorPen { get; set; }
        public int armorPenPercent { get; set; }
        public int attackDamage { get; set; }
        public int attackSpeed { get; set; }
        public int bonusArmorPenPercent { get; set; }
        public int bonusMagicPenPercent { get; set; }
        public int ccReduction { get; set; }
        public int cooldownReduction { get; set; }
        public int health { get; set; }
        public int healthMax { get; set; }
        public int healthRegen { get; set; }
        public int lifesteal { get; set; }
        public int magicPen { get; set; }
        public int magicPenPercent { get; set; }
        public int magicResist { get; set; }
        public int movementSpeed { get; set; }
        public int omnivamp { get; set; }
        public int physicalVamp { get; set; }
        public int spellVamp { get; set; }
    }

    // Estadísticas de daño en un momento dado
    public class DamageStatsDto
    {
        public int magicDamageDone { get; set; }
        public int magicDamageDoneToChampions { get; set; }
        public int magicDamageTaken { get; set; }
        public int physicalDamageDone { get; set; }
        public int physicalDamageDoneToChampions { get; set; }
        public int physicalDamageTaken { get; set; }
        public int totalDamageDone { get; set; }
        public int totalDamageDoneToChampions { get; set; }
        public int totalDamageTaken { get; set; }
        public int trueDamageDone { get; set; }
        public int trueDamageDoneToChampions { get; set; }
        public int trueDamageTaken { get; set; }
    }

    // Posición en el mapa
    public class PositionDto
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
