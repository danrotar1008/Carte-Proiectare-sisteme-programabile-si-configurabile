--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   10:06:18 10/14/2021
-- Design Name:   
-- Module Name:   C:/LUCRU/fsm1/script/fsm1/fsm1_TB.vhd
-- Project Name:  fsm1
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: fsm1
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY fsm1_TB IS
END fsm1_TB;
 
ARCHITECTURE behavior OF fsm1_TB IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT fsm1
    PORT(
         clk : IN  std_logic;
         reset : IN  std_logic;
         sw : IN  std_logic_vector(7 downto 0);
         led : OUT  std_logic_vector(7 downto 0)
        );
    END COMPONENT;
    

   --Inputs
   signal clk : std_logic := '0';
   signal reset : std_logic := '0';
   signal sw : std_logic_vector(7 downto 0) := (others => '0');

 	--Outputs
   signal led : std_logic_vector(7 downto 0);

   -- Clock period definitions
   constant clk_period : time := 10 ns;
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: fsm1 PORT MAP (
          clk => clk,
          reset => reset,
          sw => sw,
          led => led
        );

   -- Clock process definitions
   clk_process :process
   begin
		clk <= '0';
		wait for T/2;
		clk <= '1';
		wait for T/2;
   end process;
 
-- reset
-- reset activat pentru T/2
      reset <= '1', '0' after T/2;

   -- Stimulus process
   stim_proc: process

   begin		
	sw <= "00000000";
	wait for 1.5 * T;
	sw <= "00000001";
	wait for 1.5 * T;
	sw <= "00000010";
	wait for 1.5 * T;
	sw <= "00000011";
	wait for 1.5 * T;
	sw <= "00000100";
	wait for 1.5 * T;
	sw <= "00000101";
	wait for 1.5 * T;


end process;

END;
