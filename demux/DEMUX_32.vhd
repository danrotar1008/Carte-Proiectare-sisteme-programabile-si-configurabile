-- Demultiplexor de 32-biti

library ieee;
  use ieee.std_logic_1164.all;
  use ieee.numeric_std.all;

entity demux_32 is
port (
  a      : in  std_logic_vector(4 downto 0);
  enable : in  std_logic;
  y      : out std_logic_vector(31 downto 0)
);
end demux_32;

architecture behaviour of demux_32 is

begin

  demux_a_to_y: process (a, enable)
  begin
    y <= (others => '0');
    if enable = '1' then
      y(to_integer(unsigned(a))) <= '1';
    end if;
  end process;

end behaviour;
